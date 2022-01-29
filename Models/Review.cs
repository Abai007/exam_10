using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace exam_10.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ReviewText { get; set; }
        public int Grade { get; set; }
        public DateTime DateTime { get; set; }
        public string InstitutionId { get; set; }
        public Institution Institution { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
