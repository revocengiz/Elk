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
            var user = new User();

            for (int i = 0; i < 5; i++)
            {
                logger.Information("The {@User} has access to {Resource}", user, $"resource-{i}");
            }


            System.Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }

     public class User
    {
        public string Email { get; set; } = "revocengiz@gmail.com";
        public string Name { get; set; } = "Cengiz";
    }
}
