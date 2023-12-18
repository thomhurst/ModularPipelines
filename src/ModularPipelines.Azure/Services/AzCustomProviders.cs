using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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