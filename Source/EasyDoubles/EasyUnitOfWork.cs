namespace EasyDoubles;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TIKSN.Data;

/// <inheritdoc />
public class EasyUnitOfWork : IUnitOfWork
{
    private readonly AsyncServiceScope serviceScope;

    /// <summary>
    /// Initializes a new instance of the <see cref="EasyUnitOfWork"/> class.
    /// </summary>
    /// <param name="serviceScope">Service Scope.</param>
    public EasyUnitOfWork(AsyncServiceScope serviceScope)
        => this.serviceScope = serviceScope;

    /// <inheritdoc />
    public IServiceProvider Services => this.serviceScope.ServiceProvider;

    /// <inheritdoc />
    public Task CompleteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DiscardAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromException(new InvalidOperationException("Unable to track and discard changes."));
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return this.serviceScope.DisposeAsync();
    }
}
