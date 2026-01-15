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
    /// builder.Services.AddModule&lt;BuildModule&gt;();
    /// builder.Options.ExecutionMode = ExecutionMode.StopOnFirstException;
    ///
    /// var pipeline = builder.Build();
    /// var summary = await pipeline.RunAsync();
    /// </code>
    /// </example>
    public static PipelineBuilder CreateBuilder(string[]? args = null)
    {
        return new PipelineBuilder(args);
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
    /// builder.Services.AddModule&lt;BuildModule&gt;();
    /// var pipeline = builder.Build();
    /// var summary = await pipeline.RunAsync();
    /// </code>
    /// </example>
    public static PipelineBuilder CreateBuilder(PipelineBuilderOptions options)
    {
        return new PipelineBuilder(options);
    }
}
