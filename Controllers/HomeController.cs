using exam_10.Models;
using exam_10.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace exam_10.Controllers
{
    public class HomeController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private readonly ILogger<HomeController> _logger;
        private DBContext _db;
        private readonly UserManager<User> _userManager;
        public HomeController(DBContext db, ILogger<HomeController> logger, IWebHostEnvironment appEnvironment, UserManager<User> userManager) => 
            (_db,_logger, _appEnvironment, _userManager) = (db, logger, appEnvironment, userManager);
        public async Task<IActionResult> Index(int page, string name)
        {
            int pageSize = 20;   // количество элементов на странице
            if (page == 0)
                page = 1;
            _db.Reviews.ToList();
            _db.ImageModels.ToList();
            IQueryable<Institution> source;
            if (name != null)
            {
                source = _db.Institutions.Where(i => i.Name == name);
            }
            else
            {
                source = _db.Institutions;
            }
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Institutions = items,
                Institution = new Institution()
            };
            foreach (var i in viewModel.Institutions)
            {
                if (i.Reviews != null && i.Reviews.Count != 0)
                {
                    i.Rating = Rating(i.Reviews);
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Institution institution, IFormFile image)
        {
            if(institution != null)
            {
                if (image != null)
                {
                    institution.Image = "/Images/" + image.FileName;
                    using (var stream = new FileStream(_appEnvironment.WebRootPath + institution.Image, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
                _db.Institutions.Add(institution);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public async Task<IActionResult> AddImageToIns(string InstitutionId, IFormFile image)
        {
            ImageModel imageModel = new ImageModel();
            if (image != null)
            {
                imageModel.Path = "/Images/" + image.FileName;
                using (var stream = new FileStream(_appEnvironment.WebRootPath + imageModel.Path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                imageModel.InstitutionId = InstitutionId;
                _db.ImageModels.Add(imageModel);
                _db.SaveChanges();
            }
            return RedirectToAction("Detail", "Home", new { Id = InstitutionId });
        }
        public IActionResult Detail(string Id)
        {
            Institution institution = _db.Institutions.FirstOrDefault(i => i.Id == Id);
            _db.Reviews.ToList();
            _db.ImageModels.ToList();
            _db.Users.ToList();
            DetailViewModel detailViewModel = new DetailViewModel { Institution = institution, Review = new Review() };
            if(detailViewModel.Institution.Reviews != null && detailViewModel.Institution.Reviews.Count() != 0)
            {
                detailViewModel.Institution.Rating = Rating(detailViewModel.Institution.Reviews);
                foreach (var i in detailViewModel.Institution.Reviews)
                {
                    @i.User = _db.Users.FirstOrDefault(u => u.Id == i.UserId);
                }
            }
                
            
            return View(detailViewModel);
        }

        private double Rating(List<Review> reviews)
        {
            double rating = 0;
            if(reviews.Count != 0 && reviews != null)
            {
                foreach (var item in reviews)
                {
                    rating += item.Grade;
                }
                rating /= reviews.Count();
                return Math.Round(rating, 1, MidpointRounding.AwayFromZero);
            }
            return rating;
        }
        private async Task<User> CurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
        [HttpPost]
        public IActionResult CreateReview(Review review)
        {
            if(review != null)
            {
                review.UserId = CurrentUser().Result.Id;
                _db.Reviews.Add(review);
                _db.SaveChanges();
            }
            return RedirectToAction("Detail", "Home", new { Id = review.InstitutionId});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
