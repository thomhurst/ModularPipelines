using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Identifies the configured option names to inspect for secrets. Packages using this
/// registration must register every active name, including the default when used.
/// </summary>
internal sealed record OptionsRegistration(Type OptionsType, string Name, Func<IServiceProvider, object> GetValue)
{
    public static void Register<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(
        IServiceCollection services, string name)
        where T : class
    {
        services.AddSingleton(new OptionsRegistration(typeof(T), name, provider =>
            name == Microsoft.Extensions.Options.Options.DefaultName
                ? provider.GetRequiredService<IOptions<T>>().Value
                : provider.GetRequiredService<IOptionsMonitor<T>>().Get(name)));
    }
}
