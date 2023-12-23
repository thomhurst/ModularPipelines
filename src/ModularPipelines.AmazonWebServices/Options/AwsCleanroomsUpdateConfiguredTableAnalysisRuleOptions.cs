using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "update-configured-table-analysis-rule")]
public record AwsCleanroomsUpdateConfiguredTableAnalysisRuleOptions(
[property: CommandSwitch("--configured-table-identifier")] string ConfiguredTableIdentifier,
[property: CommandSwitch("--analysis-rule-type")] string AnalysisRuleType,
[property: CommandSwitch("--analysis-rule-policy")] string AnalysisRulePolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}