using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "create-custom-routing-endpoint-group")]
public record AwsGlobalacceleratorCreateCustomRoutingEndpointGroupOptions(
[property: CliOption("--listener-arn")] string ListenerArn,
[property: CliOption("--endpoint-group-region")] string EndpointGroupRegion,
[property: CliOption("--destination-configurations")] string[] DestinationConfigurations
) : AwsOptions
{
    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}