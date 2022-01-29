using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam_10.Models;

namespace exam_10.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Institution> Institutions { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public Institution Institution { get; set; }
    }
}
