using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Domain.Core;

namespace Ultimate.Services.Order.Domain.OrderAggreate
{
    //DOmain Driven Design da Business kuralları da ilgili domain modelin içinde yer alır
    //Klasik Mono Anemic Model Mimairlerinde Business Layer da Busines Logic yapılmaktadır.
    public class Address:ValueObject
    {
        //propertylerin set özelliği private yapıp dışarıdan değişimini engelliyoruz
        public string Province { get; private set; }
        public string District { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public string Line { get; private set; }

        public Address(string province, string district, string street, string zipCode, string line)
        {
            Province = province;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Province;
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}
