using CRMAPI.Data;
using CRMAPI.Repository;
using CRMAPI.Repository.IRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRMAPI.CRMMapper;
using System.Reflection;
using System.IO;

namespace CRMAPI
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddAutoMapper(typeof(CRMMappings));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("CRMOpenAPISpecDepartments",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "CRM API - Departments",
                        Version = "1",
                        Description="CRM Web Application - Departments",
                        Contact=new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email="xavyurbina88@gmail.com",
                            Name="Javier Urbina",
                            Url=new Uri("https://www.linkedin.com/in/francisco-javier-urbina-blandon-82475492/")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name="MIT License",
                            Url= new Uri("https://en.wikipedia.org/wiki/MIT_License")
                        }
                    });

                options.SwaggerDoc("CRMOpenAPISpecPositions",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "CRM API - Positions",
                        Version = "1",
                        Description = "CRM Web Application - Positions",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "xavyurbina88@gmail.com",
                            Name = "Javier Urbina",
                            Url = new Uri("https://www.linkedin.com/in/francisco-javier-urbina-blandon-82475492/")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                        }
                    });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                options.IncludeXmlComments(cmlCommentsFullPath);

            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/CRMOpenAPISpecDepartments/swagger.json", "CRM API - Departments");
                options.SwaggerEndpoint("/swagger/CRMOpenAPISpecPositions/swagger.json", "CRM API - Positions");
                options.RoutePrefix = "";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
