using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "remove-custom-routing-endpoints")]
public record AwsGlobalacceleratorRemoveCustomRoutingEndpointsOptions(
[property: CliOption("--endpoint-ids")] string[] EndpointIds,
[property: CliOption("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}