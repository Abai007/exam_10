using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace exam_10.Models
{
    public class Institution
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        public double Rating { get; set; }  
        public List<ImageModel> Images { get; set; }
        public List<Review> Reviews { get; set; }

    }
}
