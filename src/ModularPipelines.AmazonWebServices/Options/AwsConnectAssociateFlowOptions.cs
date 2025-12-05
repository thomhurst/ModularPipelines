using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "associate-flow")]
public record AwsConnectAssociateFlowOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--flow-id")] string FlowId,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}