using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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