using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "get-configured-table-analysis-rule")]
public record AwsCleanroomsGetConfiguredTableAnalysisRuleOptions(
[property: CommandSwitch("--configured-table-identifier")] string ConfiguredTableIdentifier,
[property: CommandSwitch("--analysis-rule-type")] string AnalysisRuleType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}