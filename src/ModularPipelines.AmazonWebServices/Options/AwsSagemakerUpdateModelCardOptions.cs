using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-model-card")]
public record AwsSagemakerUpdateModelCardOptions(
[property: CommandSwitch("--model-card-name")] string ModelCardName
) : AwsOptions
{
    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--model-card-status")]
    public string? ModelCardStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}