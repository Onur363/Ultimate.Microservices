using Course.Services.Catalog.Collection;
using Course.Services.Catalog.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;

namespace Course.Services.Catalog.Services.Absract
{
    public interface ICategoryService
    {
        Task<Response<CategoryDto>> GetById(string id);
        Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);
        Task<Response<List<CategoryDto>>> GetAllAsync();
    }
}
