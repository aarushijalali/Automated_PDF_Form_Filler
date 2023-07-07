using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.IO;
using Microsoft.Extensions.FileProviders;



namespace pdfreader_server
{
    public static class Startup
    {
        public static WebApplication InitializeApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;



        }
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });



            //JSON Serializer
            builder.Services
                .AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }



        private static void Configure(WebApplication app)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

        //    app.UseStaticFiles(new StaticFileOptions
        //    {
        //        FileProvider = new PhysicalFileProvider(
        //Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
        //        RequestPath = "/Photos"
        //    });




            app.UseHttpsRedirection();



            app.UseAuthorization();



            app.MapControllers();
        }
    }
}