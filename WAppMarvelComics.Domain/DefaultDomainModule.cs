using Autofac;
using WAppMarvelComics.Domain.Custom;
using WAppMarvelComics.Domain.Interfaces;
using WAppMarvelComics.Domain.Services;

namespace WAppMarvelComics.Domain
{
    public class DefaultDomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SecureUtilities>()
                .As<ISecureUtilities>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<IdentificationTypeService>()
               .As<IIdentificationTypeService>()
               .InstancePerLifetimeScope();
        }
    }
}