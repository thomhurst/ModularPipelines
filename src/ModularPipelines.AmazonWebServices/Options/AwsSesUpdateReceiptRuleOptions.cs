using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "update-receipt-rule")]
public record AwsSesUpdateReceiptRuleOptions(
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--rule")] string Rule
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}