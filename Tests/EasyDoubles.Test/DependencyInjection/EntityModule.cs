namespace EasyDoubles.Test.DependencyInjection;

using Autofac;
using EasyDoubles.Test.Repositories;

public class EntityModule : CommonModule
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        _ = builder
            .RegisterType<EntityCatalogBrandRepository>()
            .As<ICatalogBrandRepository>()
            .As<ICatalogBrandQueryRepository>()
            .InstancePerLifetimeScope();

        _ = builder
            .RegisterType<EntityCatalogItemRepository>()
            .As<ICatalogItemRepository>()
            .As<ICatalogItemQueryRepository>()
            .InstancePerLifetimeScope();

        _ = builder
            .RegisterType<EntityCatalogTypeRepository>()
            .As<ICatalogTypeRepository>()
            .As<ICatalogTypeQueryRepository>()
            .InstancePerLifetimeScope();
    }
}
