using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.Basket.Settings
{
    //Startup tarafında bu class ı Options Pattern ile set edeceğiz
    public class RedisSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
