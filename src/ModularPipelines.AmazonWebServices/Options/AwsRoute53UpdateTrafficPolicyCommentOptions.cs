using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "update-traffic-policy-comment")]
public record AwsRoute53UpdateTrafficPolicyCommentOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--comment")] string Comment,
[property: CommandSwitch("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}