using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "delete-receipt-rule")]
public record AwsSesDeleteReceiptRuleOptions(
[property: CliOption("--rule-set-name")] string RuleSetName,
[property: CliOption("--rule-name")] string RuleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}