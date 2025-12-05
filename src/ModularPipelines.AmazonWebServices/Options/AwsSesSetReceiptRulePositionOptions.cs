using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "set-receipt-rule-position")]
public record AwsSesSetReceiptRulePositionOptions(
[property: CliOption("--rule-set-name")] string RuleSetName,
[property: CliOption("--rule-name")] string RuleName
) : AwsOptions
{
    [CliOption("--after")]
    public string? After { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}