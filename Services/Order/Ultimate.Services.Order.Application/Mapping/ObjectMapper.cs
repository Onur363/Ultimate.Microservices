using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Services.Order.Application.Mapping
{
    public static class ObjectMapper
    {
        //Lazy loading kullanarak proje ayaga kaldırıldığında değil bu static sınıf çağrıldığında oluşturulmasını sağlaytan bir yöntem

        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
          {
              var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<CustomMapping>();
                });

              return config.CreateMapper();
          });

        public static IMapper Mapper => lazy.Value;
    }
}
