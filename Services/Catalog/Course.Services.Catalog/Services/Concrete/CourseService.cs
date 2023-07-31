using AutoMapper;
using Course.Services.Catalog.Collection;
using Course.Services.Catalog.Dto;
using Course.Services.Catalog.Services.Absract;
using Course.Services.Catalog.Settings;
using Mass=MassTransit; //kütüphane ye rişmek için isim verme işlemi
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;
using Ultimate.SharedCommon.Messages;

namespace Course.Services.Catalog.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Collection.Course> _courseCollection;
        private readonly IMongoCollection<Collection.Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly Mass.IPublishEndpoint publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, Mass.IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Collection.Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Collection.Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Count > 0)
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Collection.Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Collection.Course>(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return Response<CourseDto>.Fail(ServiceMessage.CourseNotFound, 404);
            }

            course.Category = await _categoryCollection.Find<Category>(c => c.Id == course.CategoryId).FirstAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Collection.Course>(x => x.UserId == userId).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }

            }
            else
            {
                courses = new List<Collection.Course>();
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Collection.Course>(courseCreateDto);
            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updatedCourse = _mapper.Map<Collection.Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updatedCourse);

            if (result == null)
            {
                return Response<NoContent>.Fail(ServiceMessage.CourseNotFound, 404);
            }

            await publishEndpoint.Publish<CourseNameChangeEvent>(new CourseNameChangeEvent()
            {
                UserId=updatedCourse.UserId,
                CourseId = updatedCourse.Id,
                UpdatedName = updatedCourse.Name
            });

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail(ServiceMessage.CourseNotFound, 404);
        }
    }
}
