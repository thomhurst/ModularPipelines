namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a module dependency conflict is detected during pipeline validation.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when the framework detects conflicting or invalid dependency
/// configurations between modules. This can occur when multiple modules have incompatible
/// dependency declarations or when the dependency graph cannot be resolved.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>During module registration validation</item>
/// <item>When dependency detection finds conflicting module references</item>
/// <item>When the same module appears multiple times in a dependency chain with different configurations</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (DependencyCollisionException ex)
/// {
///     Console.WriteLine($"Dependency conflict: {ex.Message}");
///     // Review module dependencies and resolve conflicts
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <para>
/// Review your module dependency declarations and ensure there are no conflicting
/// or duplicate dependency definitions. Check that each module is registered only once.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="CircularDependencyException"/>
public class DependencyCollisionException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyCollisionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the dependency collision.</param>
    public DependencyCollisionException(string? message) : base(message)
    {
    }
}