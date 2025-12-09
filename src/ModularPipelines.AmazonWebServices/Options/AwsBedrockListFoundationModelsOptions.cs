using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock", "list-foundation-models")]
public record AwsBedrockListFoundationModelsOptions : AwsOptions
{
    [CliOption("--by-provider")]
    public string? ByProvider { get; set; }

    [CliOption("--by-customization-type")]
    public string? ByCustomizationType { get; set; }

    [CliOption("--by-output-modality")]
    public string? ByOutputModality { get; set; }

    [CliOption("--by-inference-type")]
    public string? ByInferenceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}