using FluentResults.Extensions.AspNetCore;
using FluentResults.Samples.WebHost.Controllers;

namespace FluentResults.Samples.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AspNetCoreResult.Setup(config => config.DefaultProfile = new CustomAspNetCoreResultEndpointProfile());
                
            var builder = WebApplication.CreateBuilder(args);

            // 向容器添加服务。

            builder.Services.AddControllers();
            // 在 https://aka.ms/aspnetcore/swashbuckle 了解如何配置 Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // 配置 HTTP 请求管道。
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}