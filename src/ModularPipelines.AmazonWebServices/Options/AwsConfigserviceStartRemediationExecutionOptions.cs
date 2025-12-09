using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "start-remediation-execution")]
public record AwsConfigserviceStartRemediationExecutionOptions(
[property: CliOption("--config-rule-name")] string ConfigRuleName,
[property: CliOption("--resource-keys")] string[] ResourceKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}