using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace SerilogRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = new RedisConfiguration
            //{
            //    Host = "bigoven001.redis.cache.windows.net:6380,password=yynw11VeoVd8Ocg0WvDhav5hzl0vCkT/plqlwPmYtH8=,ssl=True,abortConnect=False",

            //};
            //config.MetaProperties.Add("_index_name", "testlog");

            //var logger = new LoggerConfiguration()
            //    .WriteTo.Redis(config)
            //    .CreateLogger();

            Log.Logger = new LoggerConfiguration()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://10.0.0.16:9200"))
    {
        AutoRegisterTemplate = true,
        IndexDecider = IndexDecider,
        BatchPostingLimit = 1
    }).CreateLogger();

            for (int i = 1; i < 1000; i++)
            {
                Log.Information("Sample event {event} happened at {time}", "test", DateTime.UtcNow);
            }
            Log.Warning("Sample log at {time} {username}", DateTime.UtcNow, "stevemur");


            Log.CloseAndFlush();
        }

        private static string IndexDecider(LogEvent logEvent, DateTimeOffset dateTimeOffset)
        {
            return "log-" + DateTime.UtcNow.Year + "-" + DateTime.UtcNow.Month + "-" + DateTime.UtcNow.Day;

        }
    }
}
