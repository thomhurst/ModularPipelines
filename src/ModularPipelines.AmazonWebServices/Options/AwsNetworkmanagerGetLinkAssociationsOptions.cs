using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-link-associations")]
public record AwsNetworkmanagerGetLinkAssociationsOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CommandSwitch("--device-id")]
    public string? DeviceId { get; set; }

    [CommandSwitch("--link-id")]
    public string? LinkId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}