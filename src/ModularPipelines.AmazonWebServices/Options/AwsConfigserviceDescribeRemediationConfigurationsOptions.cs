using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "describe-remediation-configurations")]
public record AwsConfigserviceDescribeRemediationConfigurationsOptions(
[property: CliOption("--config-rule-names")] string[] ConfigRuleNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}