using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "put-managed-rule-set-versions")]
public record AwsWafv2PutManagedRuleSetVersionsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--id")] string Id,
[property: CliOption("--lock-token")] string LockToken
) : AwsOptions
{
    [CliOption("--recommended-version")]
    public string? RecommendedVersion { get; set; }

    [CliOption("--versions-to-publish")]
    public IEnumerable<KeyValue>? VersionsToPublish { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}