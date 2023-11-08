using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MusicApi.Data;

namespace MusicApi
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
            services.AddControllers();
            services.AddMvc().AddXmlSerializerFormatters();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusicApi", Version = "v1"});
            });
            // services.AddDbContext<ApiDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
                services.AddDbContext<ApiDbContext>(option => option.UseNpgsql(Configuration.GetConnectionString("DbConnection")));
        }
                // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service Api V1"); });
            }
            //use only when the schema does not change
            //dbContext.Database.EnsureCreated();
            
            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();


            });
        }
        
    }
}
