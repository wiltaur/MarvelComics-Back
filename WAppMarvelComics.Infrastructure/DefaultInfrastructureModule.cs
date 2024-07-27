using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using WAppMarvelComics.Infrastructure.Data.Repository;
using WAppMarvelComics.Domain.Interfaces;
using Module = Autofac.Module;
using WAppMarvelComics.Infrastructure.Data;

namespace WAppMarvelComics.Infrastructure;

public class DefaultInfrastructureModule : Module
{
    private readonly List<Assembly> _assemblies = new List<Assembly>();

    protected override void Load(ContainerBuilder builder)
    {
        RegisterCommonDependencies(builder);
    }

    private void RegisterCommonDependencies(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(EfRepository<>))
          .As(typeof(IRepository<>))
          .As(typeof(IReadRepository<>))
          .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

        builder.RegisterType<UnitOfWork>()
          .As<IUnitOfWork>()
          .InstancePerLifetimeScope();

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
              .RegisterAssemblyTypes(_assemblies.ToArray())
              .AsClosedTypesOf(mediatrOpenType)
              .AsImplementedInterfaces();
        }
    }
}