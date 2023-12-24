using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "put-managed-rule-set-versions")]
public record AwsWafv2PutManagedRuleSetVersionsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--lock-token")] string LockToken
) : AwsOptions
{
    [CommandSwitch("--recommended-version")]
    public string? RecommendedVersion { get; set; }

    [CommandSwitch("--versions-to-publish")]
    public IEnumerable<KeyValue>? VersionsToPublish { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}