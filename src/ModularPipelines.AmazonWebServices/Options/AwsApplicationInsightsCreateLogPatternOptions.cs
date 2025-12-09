using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "create-log-pattern")]
public record AwsApplicationInsightsCreateLogPatternOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--pattern-set-name")] string PatternSetName,
[property: CliOption("--pattern-name")] string PatternName,
[property: CliOption("--pattern")] string Pattern,
[property: CliOption("--rank")] int Rank
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}