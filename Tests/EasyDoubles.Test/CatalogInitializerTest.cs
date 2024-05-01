namespace EasyDoubles.Test;

using EasyDoubles.Test.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

public class CatalogInitializerTest
{
    private readonly ITestOutputHelper testOutputHelper;

    public CatalogInitializerTest(ITestOutputHelper testOutputHelper)
        => this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

    [Fact]
    public async Task GivenTester_WhenInitialized_ThenAllMatch()
    {
        // Arrange
        var tester = new MultiDatabaseTester(this.testOutputHelper);
        await tester.InitializeAsync(default);

        // Act
        await tester.ForEachAsync(provider =>
            provider.GetRequiredService<ICatalogInitializer>().InitializeAsync(default));

        // Assert
        await tester.AssertAllAsync(default);
    }
}
