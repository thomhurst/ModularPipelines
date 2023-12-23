using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "get-traffic-policy")]
public record AwsRoute53GetTrafficPolicyOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}