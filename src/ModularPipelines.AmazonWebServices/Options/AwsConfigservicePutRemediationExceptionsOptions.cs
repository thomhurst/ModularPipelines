using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-remediation-exceptions")]
public record AwsConfigservicePutRemediationExceptionsOptions(
[property: CommandSwitch("--config-rule-name")] string ConfigRuleName,
[property: CommandSwitch("--resource-keys")] string[] ResourceKeys
) : AwsOptions
{
    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--expiration-time")]
    public long? ExpirationTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}