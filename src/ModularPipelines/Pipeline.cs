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
    /// <returns>A new pipeline builder instance.</returns>
    /// <example>
    /// <code>
    /// var builder = Pipeline.CreateBuilder(args);
    ///
    /// builder.AddModule&lt;BuildModule&gt;();
    /// builder.ConfigurePipelineOptions(options => options with
    /// {
    ///     FailureMode = FailureMode.FailFast,
    /// });
    ///
    /// var pipeline = await builder.BuildAsync();
    /// var summary = await pipeline.RunAsync();
    /// </code>
    /// </example>
    public static PipelineBuilder CreateBuilder(string[]? args = null)
    {
        return new PipelineBuilder(new PipelineBuilderOptions { Args = args });
    }

    /// <summary>
    /// Creates a new pipeline builder whose working directory is inferred from the calling source file.
    /// </summary>
    /// <param name="args">Optional command line arguments.</param>
    /// <param name="sourceFilePath">The calling source file path, supplied by the compiler.</param>
    /// <returns>A new pipeline builder instance.</returns>
    /// <remarks>
    /// <c>MODULAR_PIPELINES_DIRECTORY</c> can override the inferred project directory. If neither can
    /// be resolved, the process working directory is used.
    /// </remarks>
    public static PipelineBuilder CreateBuilderFromSource(
        string[]? args = null,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
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
    /// <remarks>
    /// This overload does not infer a pipeline project directory. When
    /// <see cref="PipelineBuilderOptions.WorkingDirectory"/> is
    /// unset, the configured content root is used, falling back to the process working directory.
    /// </remarks>
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
