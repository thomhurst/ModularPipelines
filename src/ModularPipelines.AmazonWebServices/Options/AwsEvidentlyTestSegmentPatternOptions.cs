using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "test-segment-pattern")]
public record AwsEvidentlyTestSegmentPatternOptions(
[property: CommandSwitch("--pattern")] string Pattern,
[property: CommandSwitch("--payload")] string Payload
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}