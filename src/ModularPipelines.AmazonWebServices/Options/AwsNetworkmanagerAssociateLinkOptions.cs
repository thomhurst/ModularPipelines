using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "associate-link")]
public record AwsNetworkmanagerAssociateLinkOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--link-id")] string LinkId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}