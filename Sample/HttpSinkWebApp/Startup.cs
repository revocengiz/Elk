using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.Http.BatchFormatters;

namespace HttpSinkWebApp
{
    public class Startup
    {
        public Startup()
        {
            Log.Logger = new LoggerConfiguration()
               .Enrich.WithProperty("CorrelationId", Guid.NewGuid().ToString())
               .Enrich.FromLogContext()
               .MinimumLevel.Information()
               .WriteTo.DurableHttp(
                   requestUri: "http://localhost:31311",
                   bufferPathFormat : "Buffer-{Hour}.json", 
                   batchFormatter: new ArrayBatchFormatter())
               .CreateLogger()
               .ForContext<HttpSinkWebApp.Program>();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory, IApplicationLifetime appLifetime)
        {
            loggerfactory.AddSerilog();
            app.UseMvc();

            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}
