using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "describe-config-rules")]
public record AwsConfigserviceDescribeConfigRulesOptions : AwsOptions
{
    [CliOption("--config-rule-names")]
    public string[]? ConfigRuleNames { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}