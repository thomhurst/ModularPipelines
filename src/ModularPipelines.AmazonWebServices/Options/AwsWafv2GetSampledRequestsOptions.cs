using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "get-sampled-requests")]
public record AwsWafv2GetSampledRequestsOptions(
[property: CommandSwitch("--web-acl-arn")] string WebAclArn,
[property: CommandSwitch("--rule-metric-name")] string RuleMetricName,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--time-window")] string TimeWindow,
[property: CommandSwitch("--max-items")] long MaxItems
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}