using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf", "get-sampled-requests")]
public record AwsWafGetSampledRequestsOptions(
[property: CommandSwitch("--web-acl-id")] string WebAclId,
[property: CommandSwitch("--rule-id")] string RuleId,
[property: CommandSwitch("--time-window")] string TimeWindow,
[property: CommandSwitch("--max-items")] long MaxItems
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}