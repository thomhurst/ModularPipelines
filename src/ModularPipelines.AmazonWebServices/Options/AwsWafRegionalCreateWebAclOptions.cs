using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "create-web-acl")]
public record AwsWafRegionalCreateWebAclOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--default-action")] string DefaultAction,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}