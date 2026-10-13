using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Applies immutable transformations alongside standard configuration and post-configuration,
/// preserving registration order, named options, validation and monitor cache invalidation.
/// </summary>
internal sealed class TransformOptionsFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(
    IEnumerable<IConfigureOptions<T>> configurations,
    IEnumerable<IPostConfigureOptions<T>> postConfigurations,
    IEnumerable<IValidateOptions<T>> validators) : IOptionsFactory<T>
    where T : class, new()
{
    public static void Configure(IServiceCollection services, Func<T, T> transform, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(transform);
        Register(services);
        // Concrete options are rooted by each package's AddOptions<TOptions> registration.
#pragma warning disable MPG0006
        services.AddSingleton<IConfigureOptions<T>>(new Configuration(name ?? Microsoft.Extensions.Options.Options.DefaultName, transform));
#pragma warning restore MPG0006
    }

    public static void PostConfigure(IServiceCollection services, Func<IServiceProvider, Func<T, T>> createTransform)
    {
        Register(services);
#pragma warning disable MPG0006
        services.AddSingleton<IPostConfigureOptions<T>>(provider => new PostConfiguration(Microsoft.Extensions.Options.Options.DefaultName, createTransform(provider)));
#pragma warning restore MPG0006
    }

    public static void Register(IServiceCollection services)
    {
        if (services.Any(descriptor => !descriptor.IsKeyedService
            && descriptor.ServiceType == typeof(IOptionsFactory<T>)
            && descriptor.ImplementationType != typeof(TransformOptionsFactory<T>)))
        {
            throw new InvalidOperationException(
                $"An incompatible IOptionsFactory<{typeof(T).Name}> is already registered. " +
                "Remove that registration before configuring immutable options.");
        }

        services.AddOptions();
        services.TryAddTransient<IOptionsFactory<T>, TransformOptionsFactory<T>>();
    }

    public T Create(string name)
    {
        var options = new T();
        foreach (var configuration in configurations)
        {
            if (configuration is Configuration transformation)
            {
                if (transformation.Name == name)
                {
                    options = Apply(transformation.Transform, options);
                }
            }
            else if (configuration is IConfigureNamedOptions<T> namedConfiguration)
            {
                namedConfiguration.Configure(name, options);
            }
            else if (name == Microsoft.Extensions.Options.Options.DefaultName)
            {
                configuration.Configure(options);
            }
        }

        foreach (var configuration in postConfigurations)
        {
            if (configuration is PostConfiguration transformation)
            {
                if (transformation.Name == name)
                {
                    options = Apply(transformation.Transform, options);
                }
            }
            else
            {
                configuration.PostConfigure(name, options);
            }
        }

        var failures = validators.Select(validator => validator.Validate(name, options))
            .Where(static result => result.Failed)
            .SelectMany(static result => result.Failures ?? []).ToArray();
        return failures.Length == 0 ? options : throw new OptionsValidationException(name, typeof(T), failures);
    }

    private static T Apply(Func<T, T> transform, T options) =>
        transform(options) ?? throw new InvalidOperationException($"The {typeof(T).Name} configuration returned null.");

    private sealed record Configuration(string Name, Func<T, T> Transform) : IConfigureNamedOptions<T>
    {
        public void Configure(T options) => throw new NotSupportedException("Immutable options require TransformOptionsFactory.");

        public void Configure(string? name, T options) => Configure(options);
    }

    private sealed record PostConfiguration(string Name, Func<T, T> Transform) : IPostConfigureOptions<T>
    {
        public void PostConfigure(string? name, T options) => throw new NotSupportedException("Immutable options require TransformOptionsFactory.");
    }
}
