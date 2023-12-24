using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rbin", "lock-rule")]
public record AwsRbinLockRuleOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--lock-configuration")] string LockConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}