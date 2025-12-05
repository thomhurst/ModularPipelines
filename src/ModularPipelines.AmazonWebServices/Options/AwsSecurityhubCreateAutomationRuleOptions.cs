using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "create-automation-rule")]
public record AwsSecurityhubCreateAutomationRuleOptions(
[property: CliOption("--rule-order")] int RuleOrder,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--description")] string Description,
[property: CliOption("--criteria")] string Criteria,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--rule-status")]
    public string? RuleStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}