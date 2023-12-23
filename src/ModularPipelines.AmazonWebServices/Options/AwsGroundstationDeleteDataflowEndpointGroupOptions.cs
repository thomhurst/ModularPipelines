using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "delete-dataflow-endpoint-group")]
public record AwsGroundstationDeleteDataflowEndpointGroupOptions(
[property: CommandSwitch("--dataflow-endpoint-group-id")] string DataflowEndpointGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}