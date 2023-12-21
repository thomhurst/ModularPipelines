using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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