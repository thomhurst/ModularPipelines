using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "set-active-receipt-rule-set")]
public record AwsSesSetActiveReceiptRuleSetOptions : AwsOptions
{
    [CommandSwitch("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}