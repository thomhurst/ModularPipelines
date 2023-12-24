using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-traffic-mirror-filter-rule")]
public record AwsEc2ModifyTrafficMirrorFilterRuleOptions(
[property: CommandSwitch("--traffic-mirror-filter-rule-id")] string TrafficMirrorFilterRuleId
) : AwsOptions
{
    [CommandSwitch("--traffic-direction")]
    public string? TrafficDirection { get; set; }

    [CommandSwitch("--rule-number")]
    public int? RuleNumber { get; set; }

    [CommandSwitch("--rule-action")]
    public string? RuleAction { get; set; }

    [CommandSwitch("--destination-port-range")]
    public string? DestinationPortRange { get; set; }

    [CommandSwitch("--source-port-range")]
    public string? SourcePortRange { get; set; }

    [CommandSwitch("--protocol")]
    public int? Protocol { get; set; }

    [CommandSwitch("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CommandSwitch("--source-cidr-block")]
    public string? SourceCidrBlock { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--remove-fields")]
    public string[]? RemoveFields { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}