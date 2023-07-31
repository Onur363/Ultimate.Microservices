using Course.Services.Catalog.Dto;
using Course.Services.Catalog.Services.Absract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.ControllerBaseSettings;

namespace Course.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController:CustomBaseController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await categoryService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")] //categories/2 ile bu metoda girmiş olacak
        public async Task<IActionResult> GetById(string id)
        {
            var response = await categoryService.GetById(id);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var response = await categoryService.CreateAsync(categoryDto);

            return CreateActionResultInstance(response);
        }
    }
}
