using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "update-rule-group")]
public record AwsWafv2UpdateRuleGroupOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--id")] string Id,
[property: CliOption("--visibility-config")] string VisibilityConfig,
[property: CliOption("--lock-token")] string LockToken
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--rules")]
    public string[]? Rules { get; set; }

    [CliOption("--custom-response-bodies")]
    public IEnumerable<KeyValue>? CustomResponseBodies { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}