using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.SharedCommon.ControllerBaseSettings
{
    //Bu metod ile API lar için gönderdiğimiz response a ait ilgili status code ile birlikte dönüş yapacak
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}
