using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "delete-custom-routing-endpoint-group")]
public record AwsGlobalacceleratorDeleteCustomRoutingEndpointGroupOptions(
[property: CliOption("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}