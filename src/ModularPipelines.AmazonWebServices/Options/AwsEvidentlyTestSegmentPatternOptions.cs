using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "test-segment-pattern")]
public record AwsEvidentlyTestSegmentPatternOptions(
[property: CliOption("--pattern")] string Pattern,
[property: CliOption("--payload")] string Payload
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}