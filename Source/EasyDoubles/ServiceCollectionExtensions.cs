namespace EasyDoubles;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TIKSN.Data;

/// <summary>
/// Extensions registers EasyDoubles components.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers EasyDoubles components.
    /// </summary>
    /// <param name="services">Collection of service descriptors.</param>
    /// <returns>Returns collection of service descriptors.</returns>
    public static IServiceCollection AddEasyDoubles(this IServiceCollection services)
    {
        services.TryAddSingleton<IUnitOfWorkFactory, EasyUnitOfWorkFactory>();
        services.TryAddSingleton<IEasyStores, EasyStores>();

        return services;
    }
}
