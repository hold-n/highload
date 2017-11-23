using StackExchange.Redis;
using System;

namespace BigData.Storage
{
    public interface IRedisConnectionManager
    {
        IDatabase GetDatabase();
    }

    public class RedisConnectionManager : IRedisConnectionManager
    {
        private static readonly Lazy<RedisConnectionManager> instance_ =
            new Lazy<RedisConnectionManager>(() => new RedisConnectionManager());
        private readonly IConnectionMultiplexer multiplexer_;

        private RedisConnectionManager()
        {
            multiplexer_ = ConnectionMultiplexer.Connect("localhost");
        }

        public static IRedisConnectionManager Instance => instance_.Value;

        public IDatabase GetDatabase()
        {
            return multiplexer_.GetDatabase();
        }
    }
}