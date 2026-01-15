namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when a plugin fails to initialize during pipeline setup.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when a plugin's <c>ConfigureServices</c> or <c>ConfigurePipeline</c>
/// method throws an exception. Plugin initialization failures are fatal and stop the pipeline
/// from executing.
/// </para>
/// <para><b>Example:</b></para>
/// <code>
/// try
/// {
///     await pipelineBuilder.ExecutePipelineAsync();
/// }
/// catch (PluginInitializationException ex)
/// {
///     Console.WriteLine($"Plugin '{ex.PluginName}' failed: {ex.Message}");
/// }
/// </code>
/// </remarks>
public class PluginInitializationException : PipelineException
{
    /// <summary>
    /// Gets the name of the plugin that failed to initialize.
    /// </summary>
    public string PluginName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginInitializationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="pluginName">The name of the plugin that failed.</param>
    public PluginInitializationException(string message, string pluginName)
        : base(message)
    {
        PluginName = pluginName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginInitializationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="pluginName">The name of the plugin that failed.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public PluginInitializationException(string message, string pluginName, Exception innerException)
        : base(message, innerException)
    {
        PluginName = pluginName;
    }
}
