using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "update-managed-rule-set-version-expiry-date")]
public record AwsWafv2UpdateManagedRuleSetVersionExpiryDateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--id")] string Id,
[property: CliOption("--lock-token")] string LockToken,
[property: CliOption("--version-to-expire")] string VersionToExpire,
[property: CliOption("--expiry-timestamp")] long ExpiryTimestamp
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}