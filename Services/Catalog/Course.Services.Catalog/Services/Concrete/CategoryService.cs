using AutoMapper;
using Course.Services.Catalog.Collection;
using Course.Services.Catalog.Dto;
using Course.Services.Catalog.Services.Absract;
using Course.Services.Catalog.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;

namespace Course.Services.Catalog.Services.Concrete
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper,IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(categories => true).ToListAsync<Category>();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var addCategory = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(addCategory);

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(addCategory), 200);
        }

        public async Task<Response<CategoryDto>> GetById(string id)
        {
            var category= await _categoryCollection.Find<Category>(x=>x.Id==id).FirstOrDefaultAsync();

            if(category==null)
            {
                return Response<CategoryDto>.Fail(ServiceMessage.CategoryNotFound,404);
            }
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}
