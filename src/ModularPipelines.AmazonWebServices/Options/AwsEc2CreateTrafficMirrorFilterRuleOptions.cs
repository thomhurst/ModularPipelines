using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-traffic-mirror-filter-rule")]
public record AwsEc2CreateTrafficMirrorFilterRuleOptions(
[property: CommandSwitch("--traffic-mirror-filter-id")] string TrafficMirrorFilterId,
[property: CommandSwitch("--traffic-direction")] string TrafficDirection,
[property: CommandSwitch("--rule-number")] int RuleNumber,
[property: CommandSwitch("--rule-action")] string RuleAction,
[property: CommandSwitch("--destination-cidr-block")] string DestinationCidrBlock,
[property: CommandSwitch("--source-cidr-block")] string SourceCidrBlock
) : AwsOptions
{
    [CommandSwitch("--destination-port-range")]
    public string? DestinationPortRange { get; set; }

    [CommandSwitch("--source-port-range")]
    public string? SourcePortRange { get; set; }

    [CommandSwitch("--protocol")]
    public int? Protocol { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}