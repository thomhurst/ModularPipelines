namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a circular dependency is detected in the module dependency graph.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown during pipeline initialization when the framework detects that
/// modules form a circular dependency chain. For example, if Module A depends on Module B,
/// and Module B depends on Module A, this creates an unresolvable cycle.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>During module registration when dependency validation runs</item>
/// <item>When <c>[DependsOn]</c> attributes or programmatic dependencies form a cycle</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="CycleTypes"/> - The list of types involved in the circular dependency</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (CircularDependencyException ex)
/// {
///     Console.WriteLine($"Circular dependency detected: {ex.Message}");
///     Console.WriteLine("Types involved in cycle:");
///     foreach (var type in ex.CycleTypes)
///     {
///         Console.WriteLine($"  - {type.Name}");
///     }
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <para>
/// Review your module dependencies and break the cycle by:
/// </para>
/// <list type="number">
/// <item>Restructuring modules to remove the circular reference</item>
/// <item>Extracting shared logic into a separate module that both can depend on</item>
/// <item>Using optional dependencies where appropriate</item>
/// </list>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="DependencyCollisionException"/>
public class CircularDependencyException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CircularDependencyException"/> class.
    /// </summary>
    public CircularDependencyException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CircularDependencyException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public CircularDependencyException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CircularDependencyException"/> class with a specified error message and cycle path.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public CircularDependencyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the types involved in the circular dependency cycle.
    /// </summary>
    public IReadOnlyList<Type> CycleTypes { get; init; } = [];

    /// <summary>
    /// Creates a CircularDependencyException with a formatted cycle path message.
    /// </summary>
    /// <param name="cyclePath">The types forming the circular dependency cycle.</param>
    /// <returns>A new CircularDependencyException with a descriptive message.</returns>
    public static CircularDependencyException CreateWithCyclePath(IReadOnlyList<Type> cyclePath)
    {
        var typeNames = cyclePath.Select(t => t.Name).ToArray();

        // Highlight the first and last (same) type to show the cycle clearly
        if (typeNames.Length >= 2)
        {
            typeNames[0] = $"**{typeNames[0]}**";
            typeNames[^1] = $"**{typeNames[^1]}**";
        }

        var pathString = string.Join(" -> ", typeNames);
        var message = $"Circular dependency detected at registration: {pathString}";

        return new CircularDependencyException(message)
        {
            CycleTypes = cyclePath
        };
    }
}
