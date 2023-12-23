using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock", "list-foundation-models")]
public record AwsBedrockListFoundationModelsOptions : AwsOptions
{
    [CommandSwitch("--by-provider")]
    public string? ByProvider { get; set; }

    [CommandSwitch("--by-customization-type")]
    public string? ByCustomizationType { get; set; }

    [CommandSwitch("--by-output-modality")]
    public string? ByOutputModality { get; set; }

    [CommandSwitch("--by-inference-type")]
    public string? ByInferenceType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}