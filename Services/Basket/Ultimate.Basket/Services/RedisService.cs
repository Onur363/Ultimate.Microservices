using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.Basket.Services
{
    public class RedisService
    {
        private readonly string host;
        private readonly int port;

        private ConnectionMultiplexer connectionMultiplexer;
        public RedisService(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Connect() => connectionMultiplexer = ConnectionMultiplexer.Connect($"{host}:{port}");

        //7Redis tarafında hazır gelen veritabanı sabitlerini seçiyoruz
        //birinci veri tabanına kaydetme işlemi yap demiş oluyoruz

        /// <summary>
        /// ShortCut =>public IDatabase GetDb(int db = 1) => connectionMultiplexer.GetDatabase(db);
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDb(int db = 1)
        { 
            return connectionMultiplexer.GetDatabase(db);
        } 
    }
}
