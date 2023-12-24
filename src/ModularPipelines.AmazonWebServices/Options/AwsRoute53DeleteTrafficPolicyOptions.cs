using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "delete-traffic-policy")]
public record AwsRoute53DeleteTrafficPolicyOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}