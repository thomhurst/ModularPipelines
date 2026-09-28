using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Caching;

/// <summary>
/// One registered transformation of <see cref="ModuleCacheOptions"/>. Transformations apply in registration order.
/// </summary>
internal sealed record ModuleCacheOptionsConfiguration(Func<ModuleCacheOptions, ModuleCacheOptions> Configure)
{
    /// <summary>
    /// Registers a transformation and the <see cref="IOptions{TOptions}"/> that applies every registered one.
    /// </summary>
    public static void Register(IServiceCollection services, Func<ModuleCacheOptions, ModuleCacheOptions> configure)
    {
        services.AddSingleton(new ModuleCacheOptionsConfiguration(configure));
        services.TryAddSingleton<IOptions<ModuleCacheOptions>>(static serviceProvider =>
            Microsoft.Extensions.Options.Options.Create(serviceProvider
                .GetServices<ModuleCacheOptionsConfiguration>()
                .Aggregate(
                    new ModuleCacheOptions(),
                    static (options, configuration) => configuration.Configure(options)
                        ?? throw new InvalidOperationException("The module cache options configuration returned null."))));
    }
}
