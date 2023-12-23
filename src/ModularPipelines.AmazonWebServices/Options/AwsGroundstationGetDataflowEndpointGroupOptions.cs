using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "get-dataflow-endpoint-group")]
public record AwsGroundstationGetDataflowEndpointGroupOptions(
[property: CommandSwitch("--dataflow-endpoint-group-id")] string DataflowEndpointGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}