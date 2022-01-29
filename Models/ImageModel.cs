using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace exam_10.Models
{
    public class ImageModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Path { get; set; }
        public string InstitutionId { get; set; }
        public Institution Institution { get; set; }
    }
}
