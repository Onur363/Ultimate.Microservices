using AutoMapper;
using Course.Services.Catalog.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Mapping
{
    public class EntityMappings:Profile
    {
        public EntityMappings()
        {
            CreateMap<Collection.Course, CourseDto>().ReverseMap();
            CreateMap<Collection.Category, CategoryDto>().ReverseMap();
            CreateMap<Collection.Feature, FeatureDto>().ReverseMap();

            CreateMap<Collection.Course, CourseCreateDto>().ReverseMap();
            CreateMap<Collection.Course, CourseUpdateDto>().ReverseMap();
        }
    }
}
