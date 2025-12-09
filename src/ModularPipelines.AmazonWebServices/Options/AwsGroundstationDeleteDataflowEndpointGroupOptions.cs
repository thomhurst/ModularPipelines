using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "delete-dataflow-endpoint-group")]
public record AwsGroundstationDeleteDataflowEndpointGroupOptions(
[property: CliOption("--dataflow-endpoint-group-id")] string DataflowEndpointGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}