using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCustomProviders
{
    public AzCustomProviders(
        AzCustomProvidersResourceProvider resourceProvider
    )
    {
        ResourceProvider = resourceProvider;
    }

    public AzCustomProvidersResourceProvider ResourceProvider { get; }
}