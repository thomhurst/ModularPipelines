using ModularPipelines.DotNet.Builders;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.DotNet.Services;

/// <summary>
/// Fluent builder extensions for IDotNet.
/// </summary>
public partial interface IDotNet
{
    /// <summary>
    /// Creates a fluent builder for the dotnet build command.
    /// </summary>
    /// <returns>A builder for configuring and executing the build command.</returns>
    /// <example>
    /// <code>
    /// var result = await context.DotNet().BuildBuilder()
    ///     .ForProject("MyProject.csproj")
    ///     .WithConfiguration("Release")
    ///     .WithFramework("net8.0")
    ///     .WithNoRestore()
    ///     .ExecuteAsync();
    /// </code>
    /// </example>
    IDotNetBuildBuilder BuildBuilder();

    /// <summary>
    /// Creates a fluent builder for the dotnet build command with initial options.
    /// Useful for modifying existing options fluently.
    /// </summary>
    /// <param name="options">The initial build options to start with.</param>
    /// <returns>A builder pre-configured with the specified options.</returns>
    /// <example>
    /// <code>
    /// var baseOptions = new DotNetBuildOptions { Configuration = "Release" };
    /// var result = await context.DotNet().BuildBuilder(baseOptions)
    ///     .WithNoRestore()
    ///     .ExecuteAsync();
    /// </code>
    /// </example>
    IDotNetBuildBuilder BuildBuilder(DotNetBuildOptions options);
}
