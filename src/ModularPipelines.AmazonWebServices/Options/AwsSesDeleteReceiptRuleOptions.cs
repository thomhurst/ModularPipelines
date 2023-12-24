using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "delete-receipt-rule")]
public record AwsSesDeleteReceiptRuleOptions(
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--rule-name")] string RuleName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}