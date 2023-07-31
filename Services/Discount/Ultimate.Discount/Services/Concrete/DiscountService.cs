using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.Discount.Services.Abstract;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.Discount.Services.Concrete
{
    public class DiscountService : IDiscountService
    {

        private readonly IConfiguration configuration;

        private readonly IDbConnection dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            this.configuration = configuration;

           dbConnection = new NpgsqlConnection(this.configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var deleteStatus = await dbConnection.ExecuteAsync("DELETE FROM discount where id=@Id", new { Id = id });

            return deleteStatus > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("discoubt not found",500);
        }

        public async Task<Response<List<Model.Discount>>> GetAll()
        {
            var discounts = await dbConnection.QueryAsync<Model.Discount>("select * from discount");

            return Response<List<Model.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Model.Discount>> GetByCodeAndUserId(string userId, string code)
        {
            var discount = await dbConnection.QueryAsync<Model.Discount>("select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = code });

            var hasDiscount = discount.FirstOrDefault();

            if(hasDiscount==null)
            {
                return Response<Model.Discount>.Fail("discount not found", 404);
            }

            return Response<Model.Discount>.Success(hasDiscount, 200);
        }

        public async Task<Response<Model.Discount>> GetById(int id)
        {
            var discount = (await dbConnection.QueryAsync<Model.Discount>("select * from discount where id=@Id", new { Id = id }))
                .SingleOrDefault();

            if (discount == null)
            {
                return Response<Model.Discount>.Fail("discount not found", 404);
            }

            return Response<Model.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Model.Discount discount)
        {
            var saveStatus = await dbConnection.ExecuteAsync("INSERT INTO discount(userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount);

            if (saveStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("an error occured while Discount is adding", 500);
        }

        public async Task<Response<NoContent>> Update(Model.Discount discount)
        {
            var updateStatus = await dbConnection.ExecuteAsync("UPDATE discount SET userid=@UserId,rate=@Rate,code=@Code WHERE id=@Id",
                new {Id=discount.Id,UserId=discount.UserId,Rate=discount.Rate,Code=discount.Code });

            if (updateStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("an error occured while Discount is updating", 500);
        }
    }
}
