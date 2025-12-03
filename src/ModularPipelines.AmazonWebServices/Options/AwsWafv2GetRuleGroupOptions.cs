using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "get-rule-group")]
public record AwsWafv2GetRuleGroupOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}