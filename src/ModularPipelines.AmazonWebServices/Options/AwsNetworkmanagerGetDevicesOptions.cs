using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-devices")]
public record AwsNetworkmanagerGetDevicesOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CommandSwitch("--device-ids")]
    public string[]? DeviceIds { get; set; }

    [CommandSwitch("--site-id")]
    public string? SiteId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}