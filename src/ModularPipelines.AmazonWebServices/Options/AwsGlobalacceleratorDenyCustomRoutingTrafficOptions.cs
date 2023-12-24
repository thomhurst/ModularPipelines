using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "deny-custom-routing-traffic")]
public record AwsGlobalacceleratorDenyCustomRoutingTrafficOptions(
[property: CommandSwitch("--endpoint-group-arn")] string EndpointGroupArn,
[property: CommandSwitch("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CommandSwitch("--destination-addresses")]
    public string[]? DestinationAddresses { get; set; }

    [CommandSwitch("--destination-ports")]
    public string[]? DestinationPorts { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}