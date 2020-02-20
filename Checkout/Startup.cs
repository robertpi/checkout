using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.BankServices;
using Checkout.PaymentStorage;
using Checkout.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Swashbuckle;

namespace Checkout
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
            // use Newtonsoft Json.NET as this is the easiest way to support Noda
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb));

            // ensure all validators are registered
            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CheckoutPaymentParametersValidator>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            // regiser noda and our custom services
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
            services.AddSingleton<IBank, BankSimulator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // not on in development mode to enable development without a self-signed certificate
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthorization();

            // active swagger and the associated UI
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
