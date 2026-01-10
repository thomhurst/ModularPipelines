namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when a circular dependency is detected in the module dependency graph during registration.
/// </summary>
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
