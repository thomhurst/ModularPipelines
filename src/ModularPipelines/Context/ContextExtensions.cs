using ModularPipelines.Enums;

namespace ModularPipelines.Context;

/// <summary>
/// Extension methods for <see cref="IModuleContext"/> providing simplified access to common operations.
/// </summary>
/// <remarks>
/// These extension methods provide a more discoverable and streamlined API for common pipeline operations.
/// They wrap the underlying service interfaces with simpler method signatures.
/// </remarks>
public static class ContextExtensions
{
    /// <summary>
    /// Gets a required service from the DI container.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <param name="context">The module context.</param>
    /// <returns>The service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the service is not registered.</exception>
    /// <example>
    /// <code>
    /// var myService = context.GetService&lt;IMyService&gt;();
    /// </code>
    /// </example>
    public static T GetService<T>(this IPipelineServices context) where T : notnull
    {
        return context.Get<T>() ?? throw new InvalidOperationException(
            $"Service '{typeof(T).FullName}' is not registered in the service provider. " +
            $"Ensure the service is registered during pipeline configuration.");
    }

    /// <summary>
    /// Tries to get a service from the DI container.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <param name="context">The module context.</param>
    /// <returns>The service instance, or null if not registered.</returns>
    /// <example>
    /// <code>
    /// var myService = context.TryGetService&lt;IMyService&gt;();
    /// if (myService != null)
    /// {
    ///     // Use the service
    /// }
    /// </code>
    /// </example>
    public static T? TryGetService<T>(this IPipelineServices context) where T : class
    {
        return context.Get<T>();
    }

    /// <summary>
    /// Gets a configuration value by key, returning null if not found.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="key">The configuration key.</param>
    /// <returns>The configuration value, or null if not found.</returns>
    /// <example>
    /// <code>
    /// var connectionString = context.GetConfigValue("ConnectionStrings:Default");
    /// </code>
    /// </example>
    public static string? GetConfigValue(this IPipelineServices context, string key)
    {
        return context.Configuration[key];
    }

    /// <summary>
    /// Gets a required configuration value by key.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="key">The configuration key.</param>
    /// <returns>The configuration value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the configuration key is not found or has no value.</exception>
    /// <example>
    /// <code>
    /// var apiKey = context.GetRequiredConfigValue("ApiKey");
    /// </code>
    /// </example>
    public static string GetRequiredConfigValue(this IPipelineServices context, string key)
    {
        var value = context.Configuration[key];
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException(
                $"Configuration key '{key}' is required but was not found or has no value. " +
                $"Ensure the configuration is properly set in appsettings.json, environment variables, or user secrets.");
        }

        return value;
    }

    /// <summary>
    /// Checks if the pipeline is running in the specified build system.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="buildSystem">The build system to check for.</param>
    /// <returns>True if running in the specified build system; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// if (context.IsRunningIn(BuildSystem.GitHubActions))
    /// {
    ///     // GitHub Actions specific logic
    /// }
    /// </code>
    /// </example>
    public static bool IsRunningIn(this IPipelineEnvironment context, BuildSystem buildSystem)
    {
        return context.BuildSystemDetector.GetCurrentBuildSystem() == buildSystem;
    }

    /// <summary>
    /// Checks if the pipeline is running locally (not in a CI/CD system).
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <returns>True if running locally; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// if (context.IsRunningLocally())
    /// {
    ///     // Skip deployment steps during local development
    /// }
    /// </code>
    /// </example>
    public static bool IsRunningLocally(this IPipelineEnvironment context)
    {
        return context.BuildSystemDetector.GetCurrentBuildSystem() == BuildSystem.Unknown;
    }

    /// <summary>
    /// Checks if the pipeline is running in any CI/CD system.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <returns>True if running in a CI/CD system; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// if (context.IsRunningInCI())
    /// {
    ///     // Enable verbose logging in CI
    /// }
    /// </code>
    /// </example>
    public static bool IsRunningInCI(this IPipelineEnvironment context)
    {
        return context.BuildSystemDetector.GetCurrentBuildSystem() != BuildSystem.Unknown;
    }
}
