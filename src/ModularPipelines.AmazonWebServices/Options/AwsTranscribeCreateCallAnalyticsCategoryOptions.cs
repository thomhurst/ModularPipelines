using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "create-call-analytics-category")]
public record AwsTranscribeCreateCallAnalyticsCategoryOptions(
[property: CommandSwitch("--category-name")] string CategoryName,
[property: CommandSwitch("--rules")] string[] Rules
) : AwsOptions
{
    [CommandSwitch("--input-type")]
    public string? InputType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}