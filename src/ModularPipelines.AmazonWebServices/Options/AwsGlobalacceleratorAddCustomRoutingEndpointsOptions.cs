using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "add-custom-routing-endpoints")]
public record AwsGlobalacceleratorAddCustomRoutingEndpointsOptions(
[property: CliOption("--endpoint-configurations")] string[] EndpointConfigurations,
[property: CliOption("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}