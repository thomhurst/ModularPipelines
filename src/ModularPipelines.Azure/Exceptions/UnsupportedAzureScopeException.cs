using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Exceptions;

/// <summary>
/// Exception thrown when an unsupported Azure scope type is encountered.
/// </summary>
public class UnsupportedAzureScopeException : NotSupportedException
{
    /// <summary>
    /// Gets the Azure scope that is not supported.
    /// </summary>
    public AzureScope Scope { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsupportedAzureScopeException"/> class.
    /// </summary>
    /// <param name="scope">The Azure scope that is not supported.</param>
    public UnsupportedAzureScopeException(AzureScope scope)
        : base($"Unsupported Azure scope type: {scope.GetType().Name}. Scope value: {scope}")
    {
        Scope = scope;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsupportedAzureScopeException"/> class.
    /// </summary>
    /// <param name="scope">The Azure scope that is not supported.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    public UnsupportedAzureScopeException(AzureScope scope, Exception? innerException)
        : base($"Unsupported Azure scope type: {scope.GetType().Name}. Scope value: {scope}", innerException)
    {
        Scope = scope;
    }
}
