namespace ModularPipelines.Exceptions;

/// <summary>
/// An internal exception thrown when a module is accessed before it has been properly initialized.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when code attempts to use a module before the framework has
/// completed its initialization. This typically occurs when work is performed in a module's
/// constructor instead of in the <c>ExecuteAsync</c> method.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When accessing module properties or methods before initialization completes</item>
/// <item>When performing work in a module constructor that should be in <c>ExecuteAsync</c></item>
/// </list>
/// <para><b>Resolution:</b></para>
/// <para>
/// Move all module work to the <c>ExecuteAsync</c> method. The constructor should only
/// be used for dependency injection field assignments:
/// </para>
/// <code>
/// public class MyModule : Module&lt;string&gt;
/// {
///     private readonly IMyService _service;
///
///     // Constructor: only assign injected dependencies
///     public MyModule(IMyService service)
///     {
///         _service = service;
///     }
///
///     // All work should happen here
///     protected override async Task&lt;string&gt; ExecuteAsync(IModuleContext context, CancellationToken token)
///     {
///         return await _service.DoWorkAsync();
///     }
/// }
/// </code>
/// <para>
/// <b>Note:</b> This is an internal exception. It indicates a programming error that should
/// be fixed during development.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
internal class ModuleNotInitializedException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleNotInitializedException"/> class.
    /// </summary>
    /// <param name="moduleType">The type of the module that was not initialized.</param>
    public ModuleNotInitializedException(Type moduleType) : base($"Module {moduleType.Name} has not been initialized. Avoid doing any work in the constructor other than assigning fields via Dependency Injection.")
    {
    }
}