using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "remove-endpoints")]
public record AwsGlobalacceleratorRemoveEndpointsOptions(
[property: CliOption("--endpoint-identifiers")] string[] EndpointIdentifiers,
[property: CliOption("--endpoint-group-arn")] string EndpointGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}