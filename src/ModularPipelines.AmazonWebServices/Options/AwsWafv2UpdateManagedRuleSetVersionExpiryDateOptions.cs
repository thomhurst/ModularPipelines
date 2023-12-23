using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "update-managed-rule-set-version-expiry-date")]
public record AwsWafv2UpdateManagedRuleSetVersionExpiryDateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--lock-token")] string LockToken,
[property: CommandSwitch("--version-to-expire")] string VersionToExpire,
[property: CommandSwitch("--expiry-timestamp")] long ExpiryTimestamp
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}