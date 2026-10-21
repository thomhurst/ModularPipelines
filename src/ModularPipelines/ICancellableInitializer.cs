using Initialization.Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines;

/// <summary>
/// Initializes a singleton service with cooperative pipeline startup cancellation.
/// </summary>
/// <remarks>
/// Register the service as a singleton, as for <see cref="IInitializer"/>. The pipeline calls
/// the token overload once, preserving initializer ordering and parallel batches.
/// Implementations must observe the token and finish all work before their task completes.
/// Startup awaits every started initializer before disposing services. Legacy services that
/// implement only <see cref="IInitializer"/> still run without a token and must finish before
/// startup can report cancellation. Calling the inherited parameterless method uses the default token.
/// </remarks>
public interface ICancellableInitializer : IInitializer
{
    /// <summary>
    /// Initializes the service, observing pipeline startup cancellation.
    /// </summary>
    /// <param name="cancellationToken">A token that cancels initialization.</param>
    /// <returns>A task representing all initialization work.</returns>
    Task InitializeAsync(CancellationToken cancellationToken);

    Task IInitializer.InitializeAsync() => InitializeAsync(CancellationToken.None);
}
