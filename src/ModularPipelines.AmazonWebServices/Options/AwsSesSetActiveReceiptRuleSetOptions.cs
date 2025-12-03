using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "set-active-receipt-rule-set")]
public record AwsSesSetActiveReceiptRuleSetOptions : AwsOptions
{
    [CliOption("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}