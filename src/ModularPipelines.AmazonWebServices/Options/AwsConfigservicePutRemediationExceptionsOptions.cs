using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-remediation-exceptions")]
public record AwsConfigservicePutRemediationExceptionsOptions(
[property: CliOption("--config-rule-name")] string ConfigRuleName,
[property: CliOption("--resource-keys")] string[] ResourceKeys
) : AwsOptions
{
    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--expiration-time")]
    public long? ExpirationTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}