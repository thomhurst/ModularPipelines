using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "update-log-pattern")]
public record AwsApplicationInsightsUpdateLogPatternOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--pattern-set-name")] string PatternSetName,
[property: CliOption("--pattern-name")] string PatternName,
[property: CliOption("--pattern")] string Pattern
) : AwsOptions
{
    [CliOption("--rank")]
    public int? Rank { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}