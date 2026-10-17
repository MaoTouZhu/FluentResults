using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using FluentResults.Samples.MediatR.MediatRLogic.PipelineBehaviors;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluentResults.Samples.MediatR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 运行时调用此方法，可在这里向容器添加服务。
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddMediatR(typeof(Startup));
        }

        // 运行时调用此方法，可在这里配置 HTTP 请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        // ConfigureContainer 用于直接注册依赖，
        // 这里使用 Autofac。它在 ConfigureServices 之后运行，
        // 因此这里的注册会覆盖 ConfigureServices 中的注册。
        // 不要自行构建容器，工厂会替你完成。
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 在这里直接向 Autofac 注册你自己的类型，例如：

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(Ping).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    // 当一个类实现多种处理器接口时，
                    // 这次注册会导致处理器被调用两次。
                    // 通常应避免让同一个类同时实现 `IRequestHandler<,>` 和 `INotificationHandler<>`，
                    // 另一种选择是移除这次调用，
                    // 另见 https://github.com/jbogard/MediatR/issues/462
                    .AsImplementedInterfaces();
            }

            // Autofac 似乎会先返回最后注册的类型
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidationPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}
