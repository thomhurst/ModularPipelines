using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "delete-provisioned-model-throughput")]
public record AwsBedrockDeleteProvisionedModelThroughputOptions(
[property: CliOption("--provisioned-model-id")] string ProvisionedModelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}