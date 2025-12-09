using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "reorder-receipt-rule-set")]
public record AwsSesReorderReceiptRuleSetOptions(
[property: CliOption("--rule-set-name")] string RuleSetName,
[property: CliOption("--rule-names")] string[] RuleNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}