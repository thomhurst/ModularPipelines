using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-remediation-exceptions")]
public record AwsConfigserviceDeleteRemediationExceptionsOptions(
[property: CliOption("--config-rule-name")] string ConfigRuleName,
[property: CliOption("--resource-keys")] string[] ResourceKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}