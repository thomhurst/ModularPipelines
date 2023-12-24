using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "delete-site")]
public record AwsNetworkmanagerDeleteSiteOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--site-id")] string SiteId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}