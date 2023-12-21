using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPrivateLink
{
    public AzPrivateLink(
        AzPrivateLinkAssociation association
    )
    {
        Association = association;
    }

    public AzPrivateLinkAssociation Association { get; }
}