using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "create-receipt-rule")]
public record AwsSesCreateReceiptRuleOptions(
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--rule")] string Rule
) : AwsOptions
{
    [CommandSwitch("--after")]
    public string? After { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}