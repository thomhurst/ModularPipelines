using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

/// <summary>
/// An internal exception used by the pipeline engine when a module execution fails.
/// </summary>
/// <remarks>
/// <para>
/// This is an internal exception used by the ModularPipelines engine to track and propagate
/// module execution failures. It wraps the original exception that caused the module to fail
/// and provides context about which module failed.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When a module's <c>ExecuteAsync</c> method throws an exception</item>
/// <item>When module initialization fails</item>
/// <item>Used internally for error aggregation and reporting</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="Module"/> - The module instance that failed (if available)</item>
/// <item><see cref="ModuleType"/> - The type of the module that failed</item>
/// <item><see cref="Exception.InnerException"/> - The original exception that caused the failure</item>
/// </list>
/// <para>
/// <b>Note:</b> This is an internal exception. External code should catch
/// <see cref="PipelineException"/> or more specific derived exceptions.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="DependencyFailedException"/>
/// <seealso cref="SubModuleFailedException"/>
internal class ModuleFailedException : PipelineException
{
    /// <summary>
    /// Gets the module that failed to execute (may be null for composition-based modules).
    /// </summary>
    public IModule? Module { get; }

    /// <summary>
    /// Gets the type of the module that failed.
    /// </summary>
    public Type ModuleType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleFailedException"/> class.
    /// </summary>
    /// <param name="module">The module that failed to execute.</param>
    /// <param name="exception">The exception that caused the module to fail.</param>
    public ModuleFailedException(IModule module, Exception exception)
        : base($"The module {module.GetType().Name} has failed.{GetInnerMessage(exception)}", exception)
    {
        Module = module;
        ModuleType = module.GetType();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleFailedException"/> class.
    /// </summary>
    /// <param name="moduleType">The type of module that failed to execute.</param>
    /// <param name="exception">The exception that caused the module to fail.</param>
    public ModuleFailedException(Type moduleType, Exception exception)
        : base($"The module {moduleType.Name} has failed.{GetInnerMessage(exception)}", exception)
    {
        ModuleType = moduleType;
    }

    private static string GetInnerMessage(Exception exception)
    {
        if (!string.IsNullOrEmpty(exception.Message))
        {
            return $"{Environment.NewLine}{Environment.NewLine}{exception.Message}";
        }

        return string.Empty;
    }
}
