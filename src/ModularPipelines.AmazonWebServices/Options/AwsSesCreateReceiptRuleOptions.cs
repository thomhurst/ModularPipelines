using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "create-receipt-rule")]
public record AwsSesCreateReceiptRuleOptions(
[property: CliOption("--rule-set-name")] string RuleSetName,
[property: CliOption("--rule")] string Rule
) : AwsOptions
{
    [CliOption("--after")]
    public string? After { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}