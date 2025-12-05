using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "update-receipt-rule")]
public record AwsSesUpdateReceiptRuleOptions(
[property: CliOption("--rule-set-name")] string RuleSetName,
[property: CliOption("--rule")] string Rule
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}