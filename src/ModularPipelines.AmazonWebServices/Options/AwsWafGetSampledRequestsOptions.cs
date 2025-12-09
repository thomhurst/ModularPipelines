using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "get-sampled-requests")]
public record AwsWafGetSampledRequestsOptions(
[property: CliOption("--web-acl-id")] string WebAclId,
[property: CliOption("--rule-id")] string RuleId,
[property: CliOption("--time-window")] string TimeWindow,
[property: CliOption("--max-items")] long MaxItems
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}