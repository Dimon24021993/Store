using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using Store.API.Config;
using Store.DAL;

namespace Store.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(env.ContentRootPath)
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)

                         .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                     {
#if DEBUG
                         options.RequireHttpsMetadata = false;
#endif
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidIssuer = AuthOptions.ISSUER,
                             ValidateAudience = false,
                             ValidAudience = AuthOptions.AUDIENCE,
                             ValidateLifetime = true,
                             IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                             ValidateIssuerSigningKey = true,
                         };
                     });


            services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DataBase")));

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddSwaggerDocument(settings =>
            {
                settings.DocumentName = "v1";
                settings.Title = "Store API";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials().Build();
            });
            app.UseSwagger(settings =>
            {
                settings.DocumentName = "v1";
                settings.Path = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUi3();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                                name: "default",
                                template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
