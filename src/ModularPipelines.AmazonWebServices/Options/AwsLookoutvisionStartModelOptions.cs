using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "start-model")]
public record AwsLookoutvisionStartModelOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--model-version")] string ModelVersion,
[property: CliOption("--min-inference-units")] int MinInferenceUnits
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--max-inference-units")]
    public int? MaxInferenceUnits { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}