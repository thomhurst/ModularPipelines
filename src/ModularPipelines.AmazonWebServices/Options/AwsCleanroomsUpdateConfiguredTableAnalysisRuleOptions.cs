using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "update-configured-table-analysis-rule")]
public record AwsCleanroomsUpdateConfiguredTableAnalysisRuleOptions(
[property: CliOption("--configured-table-identifier")] string ConfiguredTableIdentifier,
[property: CliOption("--analysis-rule-type")] string AnalysisRuleType,
[property: CliOption("--analysis-rule-policy")] string AnalysisRulePolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}