using System.Diagnostics.CodeAnalysis;

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