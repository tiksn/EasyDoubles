namespace EasyDoubles.Test.Services;

public interface ICatalogInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}
