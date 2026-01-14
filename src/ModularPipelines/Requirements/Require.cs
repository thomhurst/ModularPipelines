using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Requirements;

/// <summary>
/// Factory class for creating pipeline requirements with a fluent API.
/// </summary>
/// <remarks>
/// <para>
/// This class provides convenient factory methods for creating requirements
/// without needing to define custom requirement classes for simple cases.
/// </para>
/// <para><b>Quick Requirements:</b></para>
/// <code>
/// // Simple boolean condition
/// services.AddRequirement(Require.That(
///     context =&gt; Environment.Is64BitProcess,
///     "64-bit process required"));
///
/// // Async condition
/// services.AddRequirement(Require.ThatAsync(
///     async context =&gt; {
///         var result = await context.Command.ExecuteCommandLineTool(
///             new CommandLineToolOptions("docker", "--version"));
///         return result.ExitCode == 0;
///     },
///     "Docker must be installed"));
/// </code>
/// <para><b>Environment Variable Requirements:</b></para>
/// <code>
/// services.AddRequirement(Require.EnvironmentVariable("API_KEY", "API_KEY must be set"));
/// </code>
/// <para><b>File System Requirements:</b></para>
/// <code>
/// services.AddRequirement(Require.FileExists("/etc/config.json", "Config file must exist"));
/// services.AddRequirement(Require.DirectoryExists("./output", "Output directory must exist"));
/// </code>
/// </remarks>
public static class Require
{
    /// <summary>
    /// Creates a requirement from a synchronous condition delegate.
    /// </summary>
    /// <param name="condition">A delegate that returns true when the requirement is satisfied.</param>
    /// <param name="failureReason">The message to display when the requirement fails.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement based on the delegate.</returns>
    /// <example>
    /// <code>
    /// services.AddRequirement(Require.That(
    ///     ctx =&gt; OperatingSystem.IsWindows() || OperatingSystem.IsLinux(),
    ///     "Windows or Linux is required"));
    /// </code>
    /// </example>
    public static IPipelineRequirement That(
        Func<IPipelineHookContext, bool> condition,
        string failureReason,
        int order = 0)
        => new DelegateRequirement(condition, failureReason, order);

    /// <summary>
    /// Creates a requirement from an asynchronous condition delegate.
    /// </summary>
    /// <param name="condition">An async delegate that returns true when the requirement is satisfied.</param>
    /// <param name="failureReason">The message to display when the requirement fails.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement based on the async delegate.</returns>
    /// <example>
    /// <code>
    /// services.AddRequirement(Require.ThatAsync(
    ///     async ctx =&gt; {
    ///         var result = await ctx.Command.ExecuteCommandLineTool(
    ///             new CommandLineToolOptions("git", "--version"));
    ///         return result.ExitCode == 0;
    ///     },
    ///     "Git must be installed"));
    /// </code>
    /// </example>
    public static IPipelineRequirement ThatAsync(
        Func<IPipelineHookContext, Task<bool>> condition,
        string failureReason,
        int order = 0)
        => new DelegateRequirement(condition, failureReason, order);

    /// <summary>
    /// Creates a requirement that an environment variable is set and non-empty.
    /// </summary>
    /// <param name="variableName">The name of the environment variable.</param>
    /// <param name="failureReason">Optional custom failure message. Defaults to a standard message.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement that checks the environment variable.</returns>
    /// <example>
    /// <code>
    /// services.AddRequirement(Require.EnvironmentVariable("NUGET_API_KEY"));
    /// </code>
    /// </example>
    public static IPipelineRequirement EnvironmentVariable(
        string variableName,
        string? failureReason = null,
        int order = 0)
        => new DelegateRequirement(
            ctx => !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable(variableName)),
            failureReason ?? $"Environment variable '{variableName}' must be set",
            order);

    /// <summary>
    /// Creates a requirement that a file exists at the specified path.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <param name="failureReason">Optional custom failure message. Defaults to a standard message.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement that checks for file existence.</returns>
    /// <example>
    /// <code>
    /// services.AddRequirement(Require.FileExists("./appsettings.json"));
    /// </code>
    /// </example>
    [ExcludeFromCodeCoverage]
    public static IPipelineRequirement FileExists(
        string filePath,
        string? failureReason = null,
        int order = 0)
        => new DelegateRequirement(
            _ => File.Exists(filePath),
            failureReason ?? $"File '{filePath}' must exist",
            order);

    /// <summary>
    /// Creates a requirement that a directory exists at the specified path.
    /// </summary>
    /// <param name="directoryPath">The path to the directory.</param>
    /// <param name="failureReason">Optional custom failure message. Defaults to a standard message.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement that checks for directory existence.</returns>
    /// <example>
    /// <code>
    /// services.AddRequirement(Require.DirectoryExists("./artifacts"));
    /// </code>
    /// </example>
    [ExcludeFromCodeCoverage]
    public static IPipelineRequirement DirectoryExists(
        string directoryPath,
        string? failureReason = null,
        int order = 0)
        => new DelegateRequirement(
            _ => Directory.Exists(directoryPath),
            failureReason ?? $"Directory '{directoryPath}' must exist",
            order);

    /// <summary>
    /// Creates a requirement that the current platform matches the specified operating system.
    /// </summary>
    /// <param name="platform">The required operating system platform.</param>
    /// <param name="failureReason">Optional custom failure message. Defaults to a standard message.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement that checks the operating system.</returns>
    /// <example>
    /// <code>
    /// using System.Runtime.InteropServices;
    /// services.AddRequirement(Require.Platform(OSPlatform.Windows));
    /// </code>
    /// </example>
    public static IPipelineRequirement Platform(
        System.Runtime.InteropServices.OSPlatform platform,
        string? failureReason = null,
        int order = 0)
        => new DelegateRequirement(
            ctx => ctx.Environment.OperatingSystem == platform,
            failureReason ?? $"Operating system must be {platform}",
            order);

    /// <summary>
    /// Creates a requirement that the pipeline is running in a CI environment.
    /// </summary>
    /// <param name="failureReason">Optional custom failure message. Defaults to a standard message.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement that checks for CI environment.</returns>
    /// <example>
    /// <code>
    /// // Require running in CI for deployment modules
    /// services.AddRequirement(Require.CIEnvironment("Deployment can only run in CI"));
    /// </code>
    /// </example>
    public static IPipelineRequirement CIEnvironment(
        string? failureReason = null,
        int order = 0)
        => new DelegateRequirement(
            ctx =>
                !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("CI")) ||
                !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("TF_BUILD")) ||
                !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("GITHUB_ACTIONS")) ||
                !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("GITLAB_CI")) ||
                !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("CIRCLECI")) ||
                !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("JENKINS_URL")) ||
                !string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("TEAMCITY_VERSION")),
            failureReason ?? "This pipeline must run in a CI environment",
            order);

    /// <summary>
    /// Creates a requirement that the pipeline is NOT running in a CI environment (i.e., running locally).
    /// </summary>
    /// <param name="failureReason">Optional custom failure message. Defaults to a standard message.</param>
    /// <param name="order">The evaluation order. Lower values are evaluated first. Default is 0.</param>
    /// <returns>A pipeline requirement that checks for local environment.</returns>
    /// <example>
    /// <code>
    /// // Some operations should only run locally
    /// services.AddRequirement(Require.LocalEnvironment("This operation can only run locally"));
    /// </code>
    /// </example>
    public static IPipelineRequirement LocalEnvironment(
        string? failureReason = null,
        int order = 0)
        => new DelegateRequirement(
            ctx =>
                string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("CI")) &&
                string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("TF_BUILD")) &&
                string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("GITHUB_ACTIONS")) &&
                string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("GITLAB_CI")) &&
                string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("CIRCLECI")) &&
                string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("JENKINS_URL")) &&
                string.IsNullOrEmpty(ctx.Environment.Variables.GetEnvironmentVariable("TEAMCITY_VERSION")),
            failureReason ?? "This pipeline must run locally (not in CI)",
            order);
}
