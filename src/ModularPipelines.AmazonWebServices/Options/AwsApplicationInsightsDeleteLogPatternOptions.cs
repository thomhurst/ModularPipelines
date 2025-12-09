using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "delete-log-pattern")]
public record AwsApplicationInsightsDeleteLogPatternOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--pattern-set-name")] string PatternSetName,
[property: CliOption("--pattern-name")] string PatternName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}