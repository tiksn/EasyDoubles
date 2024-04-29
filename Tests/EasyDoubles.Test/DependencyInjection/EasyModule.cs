namespace EasyDoubles.Test.DependencyInjection;

using Autofac;
using EasyDoubles.Test.Repositories;

public class EasyModule : CommonModule
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        _ = builder
            .RegisterType<EasyCatalogBrandRepository>()
            .As<ICatalogBrandRepository>()
            .As<ICatalogBrandQueryRepository>()
            .InstancePerLifetimeScope();

        _ = builder
            .RegisterType<EasyCatalogItemRepository>()
            .As<ICatalogItemRepository>()
            .As<ICatalogItemQueryRepository>()
            .InstancePerLifetimeScope();

        _ = builder
            .RegisterType<EasyCatalogTypeRepository>()
            .As<ICatalogTypeRepository>()
            .As<ICatalogTypeQueryRepository>()
            .InstancePerLifetimeScope();
    }
}
