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
                    .WriteTo.DurableHttp(
                        requestUri: "http://localhost:31311",
                        batchPostingLimit: 1,
                        period: TimeSpan.FromSeconds(1))
                    .CreateLogger()
                    .ForContext<HttpSink.Program>();
            var user = new User();
            for (int i = 0; i < 5; i++)
            {
                log.Information("The {@User} has access to {Resource}", user, $"resource-{i}");
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
