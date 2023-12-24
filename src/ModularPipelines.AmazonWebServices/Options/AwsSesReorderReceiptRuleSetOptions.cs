using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "reorder-receipt-rule-set")]
public record AwsSesReorderReceiptRuleSetOptions(
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--rule-names")] string[] RuleNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}