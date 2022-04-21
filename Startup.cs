using Customers.Business.Repositories;
using Customers.Business.Services.Interfaces;
using Customers.Infra;
using Customers.Infra.Adapters;
using Customers.Infra.Adapters.Product;
using Customers.Infra.Adapters.Product.Interfaces;
using Customers.Infra.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Customers
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
            services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgresDatabase")));
            
            services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
            
            services.AddHttpClient<IProductAdapter, ProductAdapter>();
            services.AddOptions<ProductAdataperSettings>()
                .BindConfiguration(ProductAdataperSettings.CONFIG_NAME)
                .ValidateDataAnnotations();


            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IWishListRepository, WishListRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();


            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customers", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customers v1"));
            }

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
