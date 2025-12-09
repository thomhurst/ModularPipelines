using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "describe-remediation-exceptions")]
public record AwsConfigserviceDescribeRemediationExceptionsOptions(
[property: CliOption("--config-rule-name")] string ConfigRuleName
) : AwsOptions
{
    [CliOption("--resource-keys")]
    public string[]? ResourceKeys { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}