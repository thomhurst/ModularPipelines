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
        return new PipelineBuilder(new PipelineBuilderOptions
        {
            Args = args,
            WorkingDirectory = PipelineDirectory.TryFindPipelineProject(sourceFilePath),
        });
    }

    /// <summary>
    /// Creates a new pipeline builder with the specified options.
    /// </summary>
    /// <param name="options">The builder options.</param>
    /// <returns>A new pipeline builder instance.</returns>
    /// <example>
    /// <code>
    /// var builder = Pipeline.CreateBuilder(new PipelineBuilderOptions
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
    public static PipelineBuilder CreateBuilder(PipelineBuilderOptions options)
    {
        return new PipelineBuilder(options);
    }
}
