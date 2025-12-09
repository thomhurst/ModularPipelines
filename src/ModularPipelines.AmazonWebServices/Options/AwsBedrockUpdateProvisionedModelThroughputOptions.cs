using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "update-provisioned-model-throughput")]
public record AwsBedrockUpdateProvisionedModelThroughputOptions(
[property: CliOption("--provisioned-model-id")] string ProvisionedModelId
) : AwsOptions
{
    [CliOption("--desired-provisioned-model-name")]
    public string? DesiredProvisionedModelName { get; set; }

    [CliOption("--desired-model-id")]
    public string? DesiredModelId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}