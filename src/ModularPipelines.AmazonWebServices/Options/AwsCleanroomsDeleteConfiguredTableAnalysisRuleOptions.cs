using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "delete-configured-table-analysis-rule")]
public record AwsCleanroomsDeleteConfiguredTableAnalysisRuleOptions(
[property: CommandSwitch("--configured-table-identifier")] string ConfiguredTableIdentifier,
[property: CommandSwitch("--analysis-rule-type")] string AnalysisRuleType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}