using Common.Domain.Entities;
using Common.Logging;
using Common.Pagination;
using EventBus.Common.Configuration;
using EventBus.Common.EventBus;
using EventBus.Common.ModelsEvents;
using EventBus.Rabbit;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.MongoDB;
using Persistence.MongoDB.Configuration;
using Persistence.MongoDB.Repository;
using Persistence.Repository;
using Persistence.Repository.Filters;
using Persistence.Service.Command;
using Persistence.Service.Query;
using Service.Queue.API.BrokerHandler;
using ServiceList;
using System.Reflection;

namespace Service.Queue.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            var mongodbConnectionString = Configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>().ConnectionString;
            var rabbitConnectionString = Configuration.GetSection("Rabbit").Get<RabbitConnectionConfiguration>().RabbitConnectionString;
            // Health check            
            // - Add NuGet package: AspNetCore.HealthChecks.UI (Version 3.0.9)
            // - Add folder 'healthchecks' to project
            // - Check UI -> http://localhost:XXXXXX/healthchecks-ui#/healthchecks
            //adding health check services to container
            services.AddHealthChecks()
                        .AddCheck("loggerapi", () => HealthCheckResult.Healthy())
                        .AddMongoDb(mongodbConnectionString: mongodbConnectionString,
                                    name: "mongo",
                                    failureStatus: HealthStatus.Unhealthy)
                        .AddRabbitMQ(rabbitConnectionString: rabbitConnectionString);

            services.AddHealthChecksUI();

            services.AddControllers();

            services.AddSingleton<ApplicationDbContext>();

            services.AddSingleton<IServiceList<Logger>, ServiceList<Logger>>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IRepositoryQuery<Logger, FilterLogger>, RepositoryQuery>();
            services.AddTransient<IRepositoryCommand<Logger>, RepositoryCommand>();

            services.AddTransient<IRequestHandler<CreateLoggerCommand, Logger>, CreateLoggerCommandHandler>();
            services.AddTransient<IRequestHandler<CreateManyLoggerCommand, int>, CreateManyLoggerCommandHandler>();
            services.AddTransient<IRequestHandler<GetLoggerByIdQuery, Logger>, GetLoggerByIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetLoggerByFilterQuery, DataCollection<Logger>>, GetLoggerByFilterQueryHandler>();


            services.AddSingleton<IEventBus, RabbitEventBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var rabbitConnectionConfiguration = Configuration.GetSection("Rabbit").Get<RabbitConnectionConfiguration>();

                return new RabbitEventBus(
                    sp.GetService<IMediator>(),
                    scopeFactory,
                    rabbitConnectionConfiguration);
            });
            services.AddTransient<EventLoggerHandler>();

            services.AddTransient<IEventHandler<LoggerEventQueue>, EventLoggerHandler>();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Service Queue Logger",
                    Description = "Queue Logger API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Check url -> https://my.papertrailapp.com/events
            loggerFactory.AddSyslog(
                Configuration.GetValue<string>("Papertrail:host"),
                Configuration.GetValue<int>("Papertrail:port"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI();
            });

            app.UseSwagger();
            app.UseSwaggerUI(s => {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1 docs");
                s.RoutePrefix = string.Empty;
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<LoggerEventQueue, EventLoggerHandler>();
        }
    }
}
