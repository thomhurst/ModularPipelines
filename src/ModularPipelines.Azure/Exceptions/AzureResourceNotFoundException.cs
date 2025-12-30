using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Exceptions;

/// <summary>
/// Exception thrown when an Azure resource cannot be found.
/// </summary>
public class AzureResourceNotFoundException : InvalidOperationException
{
    /// <summary>
    /// Gets the Azure resource identifier that was not found.
    /// </summary>
    public AzureResourceIdentifier ResourceIdentifier { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureResourceNotFoundException"/> class.
    /// </summary>
    /// <param name="resourceIdentifier">The Azure resource identifier that was not found.</param>
    public AzureResourceNotFoundException(AzureResourceIdentifier resourceIdentifier)
        : base($"Azure resource not found: {resourceIdentifier}")
    {
        ResourceIdentifier = resourceIdentifier;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureResourceNotFoundException"/> class.
    /// </summary>
    /// <param name="resourceIdentifier">The Azure resource identifier that was not found.</param>
    /// <param name="innerException">The inner exception that caused this exception.</param>
    public AzureResourceNotFoundException(AzureResourceIdentifier resourceIdentifier, Exception? innerException)
        : base($"Azure resource not found: {resourceIdentifier}", innerException)
    {
        ResourceIdentifier = resourceIdentifier;
    }
}
