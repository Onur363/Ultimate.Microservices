using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ultimate.WebCore.Models;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            //StartUp içerisinde ilgili HttpClient içerisine BaseAddress tanımladığımız için geri kalan değeri yazıyoruz
            return await httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");
        }
    }
}
