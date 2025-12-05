using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "batch-get-flow-association")]
public record AwsConnectBatchGetFlowAssociationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--resource-ids")] string[] ResourceIds
) : AwsOptions
{
    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}