using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.Discount.Model
{
    //PostgreSql içeriisnde discount table ile bu modelimiz maplenecek
    [Dapper.Contrib.Extensions.Table("discount")]
    public class Discount
    {
        public int Id{ get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
