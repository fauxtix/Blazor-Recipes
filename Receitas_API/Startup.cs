using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Receitas_API.Context;
using Receitas_API.Data;
using Receitas_API.Services.Implementations;
using Receitas_API.Services.Interfaces;
using Receitas_API.Services.Interfaces.DapperContext;
using Syncfusion.Blazor;

namespace Receitas_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();         
            services.AddServerSideBlazor();
            services.AddSingleton(Configuration);
            var sqlConnectionConfiguration = new SqlConnectionConfiguration(Configuration.GetConnectionString("RecipesConnection"));
            services.AddSingleton(sqlConnectionConfiguration);

            services.AddTransient<IDapperContext, DapperContext>();
            services.AddTransient<IBrasilianRecipes_Dapper, BrasilianRecipes_Dapper>();
            services.AddTransient<ITastyApiRecipesService, TastyApiRecipesService>();
            services.AddTransient<ISpoonacularService, SpoonacularService>();
            services.AddTransient<IBrasilianRecipesIIService, BrasilianRecipesIIService>();
            services.AddTransient<IMyRecipesService, MyRecipesService>();

            services.AddSyncfusionBlazor();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            Syncfusion.Licensing.SyncfusionLicenseProvider
                .RegisterLicense("NzE3NTQ2QDMyMzAyZTMyMmUzMENIaUJ0QmhUQUg1MVhzMEtLMEp6SDFQdEZ1SkFvZFloMXFHUDEzdmlWUkk9");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
