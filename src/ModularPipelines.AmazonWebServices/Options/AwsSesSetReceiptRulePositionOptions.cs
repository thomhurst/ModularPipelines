using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "set-receipt-rule-position")]
public record AwsSesSetReceiptRulePositionOptions(
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--rule-name")] string RuleName
) : AwsOptions
{
    [CommandSwitch("--after")]
    public string? After { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}