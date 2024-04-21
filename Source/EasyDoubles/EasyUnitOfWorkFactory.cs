namespace EasyDoubles;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TIKSN.Data;

/// <inheritdoc />
public class EasyUnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyUnitOfWorkFactory"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service Provider.</param>
    public EasyUnitOfWorkFactory(IServiceProvider serviceProvider)
        => this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <inheritdoc />
    public Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken)
    {
        var serviceScope = this.serviceProvider.CreateAsyncScope();
        return Task.FromResult<IUnitOfWork>(new EasyUnitOfWork(serviceScope));
    }
}
