using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "delete-rule-group")]
public record AwsWafv2DeleteRuleGroupOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--id")] string Id,
[property: CliOption("--lock-token")] string LockToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}