using Serilog;
using System;

namespace HttpSink
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger log = new LoggerConfiguration()
                    .Enrich.WithProperty("CorrelationId", Guid.NewGuid().ToString())
                    .Enrich.FromLogContext()
                    .MinimumLevel.Verbose()
                    .WriteTo.DurableHttp("http://localhost:31311")
                    .CreateLogger()
                    .ForContext<HttpSink.Program>();

            for (int i = 0; i < 5; i++)
            {
                log.Information("The {user} has access to {resource}", "cengiz", $"resource-{i}");
            }

            Console.ReadLine();
        }
    }
}
