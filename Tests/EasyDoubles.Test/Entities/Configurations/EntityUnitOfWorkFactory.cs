namespace EasyDoubles.Test.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TIKSN.Data.EntityFrameworkCore;

public class EntityUnitOfWorkFactory : EntityUnitOfWorkFactoryBase
{
    private readonly CatalogContext context;

    public EntityUnitOfWorkFactory(IServiceProvider serviceProvider)
        : base(serviceProvider)
        => this.context = serviceProvider.GetRequiredService<CatalogContext>();

    protected override DbContext[] GetContexts() => [this.context];
}
