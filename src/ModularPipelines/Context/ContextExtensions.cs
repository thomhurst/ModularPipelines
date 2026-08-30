using ModularPipelines.Context;
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
    /// Gets a configuration value by key, returning null if not found.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <param name="key">The configuration key.</param>
    /// <returns>The configuration value, or null if not found.</returns>
    /// <example>
    /// <code>
    /// var connectionString = context.GetConfigValue("ConnectionStrings:Default");
    /// </code>
    /// </example>
    public static string? GetConfigValue(this IPipelineContext context, string key)
    {
        return context.Services.Configuration[key];
    }

    /// <summary>
    /// Gets a required configuration value by key.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <param name="key">The configuration key.</param>
    /// <returns>The configuration value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the configuration key is not found or has no value.</exception>
    /// <example>
    /// <code>
    /// var apiKey = context.GetRequiredConfigValue("ApiKey");
    /// </code>
    /// </example>
    public static string GetRequiredConfigValue(this IPipelineContext context, string key)
    {
        var value = context.Services.Configuration[key];
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
    /// <param name="context">The pipeline context.</param>
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
    public static bool IsRunningIn(this IPipelineContext context, BuildSystem buildSystem)
    {
        return context.Environment.BuildSystem.Current == buildSystem;
    }

    /// <summary>
    /// Checks if the pipeline is running locally (not in a CI/CD system).
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>True if running locally; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// if (context.IsRunningLocally())
    /// {
    ///     // Skip deployment steps during local development
    /// }
    /// </code>
    /// </example>
    public static bool IsRunningLocally(this IPipelineContext context)
    {
        return !context.Environment.BuildSystem.IsBuildServer;
    }

    /// <summary>
    /// Checks if the pipeline is running in any CI/CD system.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>True if running in a CI/CD system; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// if (context.IsRunningInCI())
    /// {
    ///     // Enable verbose logging in CI
    /// }
    /// </code>
    /// </example>
    public static bool IsRunningInCI(this IPipelineContext context)
    {
        return context.Environment.BuildSystem.IsBuildServer;
    }
}
