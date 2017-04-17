using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PGCoverageApi.DataContext;
using Microsoft.EntityFrameworkCore;
using PGCoverageApi.Repository;
using Serilog;

namespace PGCoverageApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            CurrentEnvironment = env;
            Configuration = builder.Build();
        }

        private IHostingEnvironment CurrentEnvironment { get; set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDbContext<CoverageContext>(opt => { opt.UseNpgsql(Configuration.GetConnectionString("CoverageContext")); },ServiceLifetime.Singleton);
            services.AddSingleton<Serilog.ILogger>(x => new LoggerConfiguration()
                                                        .MinimumLevel.Debug()
                                                        .WriteTo.RollingFile(Path.Combine(CurrentEnvironment.ContentRootPath, "Logs/myapp-{Date}.txt"))
                                                        .CreateLogger());
            services.AddSingleton<IChannelRepository, ChannelRepository>();
            services.AddSingleton<IChannelRepository, ChannelRepository>();
            services.AddSingleton<IRegionRepository, RegionRepository>();
            services.AddSingleton<IBranchRepository, BranchRepository>();
            services.AddSingleton<IRepRepository, RepRepository>();
            services.AddSingleton<IEntityCodeRepository, EntityCodeRepository>();
            services.AddSingleton<IGroupRepository, GroupRepository>();
            services.AddSingleton<IGroupRelationRepository, GroupRelationRepository>();
            services.AddSingleton<IInvestorRepository, InvestorRepository>();
            services.AddSingleton<IInvestorRelationRepository, InvestorRelationRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            app.UseMvc();
        }
    }
}
