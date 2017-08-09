using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace ElasticSink
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
            });


            var logger = loggerConfig.CreateLogger();

            for (int i = 0; i < 5; i++)
            {
                logger.Information("The {user} has access to {resource}", "cengiz", $"resource-{i}");
            }
            Console.ReadLine();

        }
    }
}
