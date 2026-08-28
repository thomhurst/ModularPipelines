using System.Runtime.CompilerServices;

namespace ModularPipelines;

/// <summary>
/// Provides static methods for creating pipeline builders.
/// </summary>
public static class Pipeline
{
    /// <summary>
    /// Creates a new pipeline builder.
    /// </summary>
    /// <param name="args">Optional command line arguments.</param>
    /// <param name="sourceFilePath">The calling source file path, supplied by the compiler.</param>
    /// <returns>A new pipeline builder instance.</returns>
    /// <example>
    /// <code>
    /// var builder = Pipeline.CreateBuilder(args);
    ///
    /// builder.AddModule&lt;BuildModule&gt;();
    /// builder.ConfigurePipelineOptions(options => options with
    /// {
    ///     ExecutionMode = ExecutionMode.StopOnFirstException,
    /// });
    ///
    /// var pipeline = await builder.BuildAsync();
    /// var summary = await pipeline.RunAsync();
    /// </code>
    /// </example>
    public static PipelineBuilder CreateBuilder(
        string[]? args = null,
        [CallerFilePath] string sourceFilePath = "")
    {
        return CreateBuilder(new PipelineBuilderSettings
        {
            Args = args,
        }, sourceFilePath);
    }

    /// <summary>
    /// Creates a new pipeline builder with the specified settings.
    /// </summary>
    /// <param name="settings">The builder settings.</param>
    /// <param name="sourceFilePath">The calling source file path, supplied by the compiler.</param>
    /// <returns>A new pipeline builder instance.</returns>
    /// <remarks>
    /// When <see cref="PipelineBuilderSettings.WorkingDirectory"/> is unset, the configured content
    /// root is used when available, then the calling source file's project directory, and finally
    /// the process working directory.
    /// </remarks>
    /// <example>
    /// <code>
    /// var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
    /// {
    ///     Args = args,
    ///     EnvironmentName = "Development"
    /// });
    ///
    /// builder.AddModule&lt;BuildModule&gt;();
    /// var pipeline = await builder.BuildAsync();
    /// var summary = await pipeline.RunAsync();
    /// </code>
    /// </example>
    public static PipelineBuilder CreateBuilder(
        PipelineBuilderSettings settings,
        [CallerFilePath] string sourceFilePath = "")
    {
        ArgumentNullException.ThrowIfNull(settings);

        return new PipelineBuilder(settings with
        {
            WorkingDirectory = settings.WorkingDirectory
                               ?? settings.ContentRootPath
                               ?? PipelineDirectory.TryFindPipelineProject(sourceFilePath),
        });
    }

    internal static PipelineBuilder CreateBuilderWithoutProjectInference(PipelineBuilderSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return new PipelineBuilder(settings);
    }
}
