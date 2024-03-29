using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using SGVisitasTecnicasASPCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Repositories;
using SGVisitasTecnicasASPCore.Data;

namespace SGVisitasTecnicasASPCore
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
            var connString = Configuration.GetConnectionString("SgvtDB");
            services.AddDbContext<SgvtDB>(options => options.UseMySQL(connString));
            FastReport.Utils.RegisteredObjects.AddConnection(typeof(MySqlDataConnection));
            services.AddControllersWithViews();

            services.AddScoped<ICategorias, CategoriasRepo>();
            services.AddScoped<IUnidades, UnidadesRepo>();
            services.AddScoped<IMarcas, MarcasRepo>();
            services.AddScoped<IProductos, ProductosRepo>();
            services.AddScoped<IEmpleados, EmpleadosRepo>();
            services.AddScoped<IClientes, ClientesRepo>();
            services.AddScoped<ICotizaciones, CotizacionesRepo>();
            services.AddScoped<IVentas, VentasRepo>();
            services.AddScoped<IVisitas, VisitasRepo>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.LoginPath = "/Access/Index";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                option.AccessDeniedPath = "/Home/Privacy";
            });
            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 4 MB
                options.MultipartBodyLengthLimit = 4194304;
            });
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

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
