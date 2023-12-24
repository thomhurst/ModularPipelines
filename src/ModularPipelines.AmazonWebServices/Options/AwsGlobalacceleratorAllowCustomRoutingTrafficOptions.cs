using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "allow-custom-routing-traffic")]
public record AwsGlobalacceleratorAllowCustomRoutingTrafficOptions(
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