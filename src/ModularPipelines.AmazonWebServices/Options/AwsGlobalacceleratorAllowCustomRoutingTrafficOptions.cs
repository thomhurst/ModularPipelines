using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "allow-custom-routing-traffic")]
public record AwsGlobalacceleratorAllowCustomRoutingTrafficOptions(
[property: CliOption("--endpoint-group-arn")] string EndpointGroupArn,
[property: CliOption("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CliOption("--destination-addresses")]
    public string[]? DestinationAddresses { get; set; }

    [CliOption("--destination-ports")]
    public string[]? DestinationPorts { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}