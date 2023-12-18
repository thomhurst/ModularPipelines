using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzResourcemanagement
{
    public AzResourcemanagement(
        AzResourcemanagementPrivateLink privateLink
    )
    {
        PrivateLink = privateLink;
    }

    public AzResourcemanagementPrivateLink PrivateLink { get; }
}

