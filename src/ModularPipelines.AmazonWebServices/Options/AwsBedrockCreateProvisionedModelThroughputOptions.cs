using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "create-provisioned-model-throughput")]
public record AwsBedrockCreateProvisionedModelThroughputOptions(
[property: CliOption("--model-units")] int ModelUnits,
[property: CliOption("--provisioned-model-name")] string ProvisionedModelName,
[property: CliOption("--model-id")] string ModelId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--commitment-duration")]
    public string? CommitmentDuration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}