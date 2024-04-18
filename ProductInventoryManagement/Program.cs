using ProductInventoryManagement.Data;
using ProductInventoryManagement.Filter;
using ProductInventoryManagement.Repositories;
using Serilog;
using System;

namespace ProductInventoryManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IProductInventoryManagementRepository, ProductInventoryManagementRepository>();
            builder.Services.AddDbContext<ProductInventoryManagementDbContext>();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProductInventoryManagementExceptionFilter());
            });
            builder.Services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ProductInventoryManagement.xml");
                c.IncludeXmlComments(filePath);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
              
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            logger.Information("Application started successfully.");
        }
    }
}
