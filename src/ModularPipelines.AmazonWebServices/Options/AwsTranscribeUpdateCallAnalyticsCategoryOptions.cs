using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "update-call-analytics-category")]
public record AwsTranscribeUpdateCallAnalyticsCategoryOptions(
[property: CliOption("--category-name")] string CategoryName,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--input-type")]
    public string? InputType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}