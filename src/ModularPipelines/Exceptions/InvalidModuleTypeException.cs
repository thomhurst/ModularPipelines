using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a type is expected to be a module but does not implement <c>IModule</c>.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when the framework encounters a type that was expected to be
/// a valid module but does not implement the <c>IModule</c> interface. Modules must inherit
/// from <c>Module&lt;T&gt;</c> or implement <c>IModule</c> directly.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When registering a type as a module that does not implement <c>IModule</c></item>
/// <item>When dependency resolution encounters an invalid module type</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="InvalidType"/> - The type that was expected to be a module but is not</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (InvalidModuleTypeException ex)
/// {
///     Console.WriteLine($"Invalid module type: {ex.InvalidType.FullName}");
///     Console.WriteLine("Ensure the type inherits from Module&lt;T&gt;");
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <para>
/// Ensure your module class inherits from <c>Module&lt;T&gt;</c> where T is the result type:
/// </para>
/// <code>
/// public class MyModule : Module&lt;string&gt;
/// {
///     protected override async Task&lt;string&gt; ExecuteAsync(IModuleContext context, CancellationToken token)
///     {
///         return "result";
///     }
/// }
/// </code>
/// </remarks>
/// <seealso cref="PipelineException"/>
[ExcludeFromCodeCoverage]
public class InvalidModuleTypeException : PipelineException
{
    /// <summary>
    /// Gets the type that was expected to be a module but is not.
    /// </summary>
    public Type InvalidType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidModuleTypeException"/> class.
    /// </summary>
    /// <param name="invalidType">The type that does not implement IModule.</param>
    public InvalidModuleTypeException(Type invalidType)
        : base($"{invalidType.FullName} is not a Module (does not implement IModule)")
    {
        InvalidType = invalidType;
    }
}
