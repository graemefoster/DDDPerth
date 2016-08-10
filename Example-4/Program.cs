using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using DDDPerth.Features.Assessment;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DDDPerth
{
    public class Example4
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
 
            host.Run();
        }
    }

    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
 
        public IServiceProvider ConfigureServices(IServiceCollection services) {

            services.AddMvc().AddXmlDataContractSerializerFormatters();
            services.AddSwaggerGen();

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                    .As(t => t.GetInterfaces()
                    .Where(a => a.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                    .Select(a => new Autofac.Core.KeyedService("commandHandler", a)));

            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                    .AsClosedTypesOf(typeof(IValidate<>));

            // builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsClosedTypesOf(typeof(IRequestHandler<,>))
            //     .Named("handler", typeof(IRequestHandler<,>));

            builder.RegisterType<Mediator>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(
                    typeof(ValidationDecorator<,>),
                    typeof(IRequestHandler<,>),
                    fromKey: "commandHandler");

            builder.Register<SingleInstanceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => c.Resolve(t);
                });

            builder.Register<MultiInstanceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                });

            builder.RegisterType<MultiInstanceFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public void Configure(
            IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole();

            app.UseMvc();
            app.UseSwaggerUi();
            app.UseSwagger();

            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
   }}
