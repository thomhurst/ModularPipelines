using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "list-traffic-policies")]
public record AwsRoute53ListTrafficPoliciesOptions : AwsOptions
{
    [CommandSwitch("--traffic-policy-id-marker")]
    public string? TrafficPolicyIdMarker { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}