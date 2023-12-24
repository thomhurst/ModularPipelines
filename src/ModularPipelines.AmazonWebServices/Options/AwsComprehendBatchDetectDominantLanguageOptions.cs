using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "batch-detect-dominant-language")]
public record AwsComprehendBatchDetectDominantLanguageOptions(
[property: CommandSwitch("--text-list")] string[] TextList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}