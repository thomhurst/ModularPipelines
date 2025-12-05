using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "get-provisioned-model-throughput")]
public record AwsBedrockGetProvisionedModelThroughputOptions(
[property: CliOption("--provisioned-model-id")] string ProvisionedModelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}