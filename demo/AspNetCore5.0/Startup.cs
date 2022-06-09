using System.Linq;
using AspNetCore5._0.Data;
using AspNetCore5._0.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore5._0
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
            services.AddServicesOfAllTypes();

            string connectionString1 = Configuration.GetConnectionString("Demo1DbConnection");
            string connectionString2 = Configuration.GetConnectionString("Demo2DbConnection");

            services.AddDbContext<Demo1DbContext>(option => option.UseSqlServer(connectionString1));
            services.AddDbContext<Demo2DbContext>(option => option.UseSqlServer(connectionString2));


            services.AddGenericRepository<Demo1DbContext>(); // Call it just after registering your DbConext.
            services.AddGenericRepository<Demo2DbContext>(); // Call it just after registering your DbConext.


            services.AddQueryRepository<Demo1DbContext>();
            services.AddQueryRepository<Demo2DbContext>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            Demo1DbContext dbContext1 = serviceScope.ServiceProvider.GetRequiredService<Demo1DbContext>();
            dbContext1.Database.Migrate();

            bool isExits1 = dbContext1.Set<Department>().Any();
            if (isExits1 == false)
            {
                Department department = new Department()
                {
                    Name = "Software Development",
                };
                dbContext1.Add(department);
                dbContext1.SaveChanges();
            }

            Demo2DbContext dbContext2 = serviceScope.ServiceProvider.GetRequiredService<Demo2DbContext>();
            dbContext2.Database.Migrate();

            bool isExits2 = dbContext2.Set<Department>().Any();
            if (isExits2 == false)
            {
                Department department = new Department()
                {
                    Name = "Software Development",
                };
                dbContext2.Add(department);
                dbContext2.SaveChanges();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
