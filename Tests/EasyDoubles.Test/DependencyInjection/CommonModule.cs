namespace EasyDoubles.Test.DependencyInjection;

using Autofac;
using EasyDoubles.Test.Services;

public class CommonModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        _ = builder
            .RegisterType<CatalogInitializer>()
            .As<ICatalogInitializer>()
            .InstancePerLifetimeScope();

        _ = builder
            .RegisterInstance(new Random(1515643399))
            .As<Random>()
            .SingleInstance();
    }
}
