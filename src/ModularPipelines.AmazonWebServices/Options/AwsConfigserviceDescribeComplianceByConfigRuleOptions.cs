using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "describe-compliance-by-config-rule")]
public record AwsConfigserviceDescribeComplianceByConfigRuleOptions : AwsOptions
{
    [CliOption("--config-rule-names")]
    public string[]? ConfigRuleNames { get; set; }

    [CliOption("--compliance-types")]
    public string[]? ComplianceTypes { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}