namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a module declares a dependency on itself.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when a module's dependency declarations include a reference
/// to itself, which creates an invalid self-referential dependency that cannot be resolved.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When a module has <c>[DependsOn&lt;T&gt;]</c> where T is the same module type</item>
/// <item>When <c>context.GetModule&lt;T&gt;()</c> is called from within module T</item>
/// <item>When programmatic dependencies include a self-reference</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (ModuleReferencingSelfException ex)
/// {
///     Console.WriteLine($"Self-reference detected: {ex.Message}");
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <para>
/// Remove the self-referential dependency. A module cannot depend on itself:
/// </para>
/// <code>
/// // Wrong - self-reference
/// [DependsOn&lt;MyModule&gt;]
/// public class MyModule : Module&lt;string&gt; { }
///
/// // Correct - depend on other modules
/// [DependsOn&lt;OtherModule&gt;]
/// public class MyModule : Module&lt;string&gt; { }
/// </code>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="CircularDependencyException"/>
public class ModuleReferencingSelfException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleReferencingSelfException"/> class.
    /// </summary>
    /// <param name="message">The message describing the self-reference error.</param>
    public ModuleReferencingSelfException(string? message) : base(message)
    {
    }
}