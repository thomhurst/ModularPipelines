using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "delete-configured-table-analysis-rule")]
public record AwsCleanroomsDeleteConfiguredTableAnalysisRuleOptions(
[property: CliOption("--configured-table-identifier")] string ConfiguredTableIdentifier,
[property: CliOption("--analysis-rule-type")] string AnalysisRuleType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}