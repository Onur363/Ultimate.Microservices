using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.WebCore.Models.Catalog
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }


        [Display(Name = "Kurs İsmi")]
        public string Name { get; set; }

        [Display(Name = "Kurs Açıklaması")]
        
        public string Description { get; set; }

        [Display(Name = "Kurs Fiyatı")]
        
        public decimal Price { get; set; }


        public string UserId { get; set; }

        public string Picture { get; set; }

        [Display(Name = "Süresi")]
        
        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kategorisi")]
        
        public string CategoryId { get; set; }

        [Display(Name = "Resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
