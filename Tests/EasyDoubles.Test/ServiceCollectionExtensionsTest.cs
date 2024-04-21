namespace EasyDoubles.Test;

using Microsoft.Extensions.DependencyInjection;
using TIKSN.Data;
using Xunit;

public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void GivenRegisteredServices_WhenServicesResolved_ThenServicesShouldNotBeNull()
    {
        // Arrange
        var services = new ServiceCollection();
        _ = services.AddEasyDoubles();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var easyStores = serviceProvider.GetService<IEasyStores>();
        var unitOfWorkFactory = serviceProvider.GetService<IUnitOfWorkFactory>();

        // Assert
        Assert.NotNull(easyStores);
        Assert.NotNull(unitOfWorkFactory);
    }
}
