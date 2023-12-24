using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "update-call-analytics-category")]
public record AwsTranscribeUpdateCallAnalyticsCategoryOptions(
[property: CommandSwitch("--category-name")] string CategoryName,
[property: CommandSwitch("--rules")] string[] Rules
) : AwsOptions
{
    [CommandSwitch("--input-type")]
    public string? InputType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}