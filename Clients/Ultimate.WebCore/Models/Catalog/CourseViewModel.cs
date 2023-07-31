using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.WebCore.Models.Catalog
{
    public class CourseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string ShortDescription => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;
        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }
        public string ShortPictureUrl { get; set; }

        public DateTime CreatedTime { get; set; }

        public FeatureViewModel Feature { get; set; }

        public string CategoryId { get; set; }

        public CatagoryViewModel Category { get; set; }
    }
}
