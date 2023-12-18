using System.Diagnostics.CodeAnalysis;

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