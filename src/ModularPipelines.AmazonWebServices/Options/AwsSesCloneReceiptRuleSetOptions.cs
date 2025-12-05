using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "clone-receipt-rule-set")]
public record AwsSesCloneReceiptRuleSetOptions(
[property: CliOption("--rule-set-name")] string RuleSetName,
[property: CliOption("--original-rule-set-name")] string OriginalRuleSetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}