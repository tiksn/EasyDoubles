namespace EasyDoubles.Test;

using EasyDoubles.Test.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class CatalogInitializerTest
{
    [Fact]
    public async Task GivenTester_WhenInitialized_ThenAssertAll()
    {
        // Arrange
        var tester = new MultiDatabaseTester();
        await tester.InitializeAsync(default);

        // Act
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Assert
        await tester.AssertAllAsync(default);
    }
}
