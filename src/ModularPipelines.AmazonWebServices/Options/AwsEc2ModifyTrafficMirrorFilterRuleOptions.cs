using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-traffic-mirror-filter-rule")]
public record AwsEc2ModifyTrafficMirrorFilterRuleOptions(
[property: CliOption("--traffic-mirror-filter-rule-id")] string TrafficMirrorFilterRuleId
) : AwsOptions
{
    [CliOption("--traffic-direction")]
    public string? TrafficDirection { get; set; }

    [CliOption("--rule-number")]
    public int? RuleNumber { get; set; }

    [CliOption("--rule-action")]
    public string? RuleAction { get; set; }

    [CliOption("--destination-port-range")]
    public string? DestinationPortRange { get; set; }

    [CliOption("--source-port-range")]
    public string? SourcePortRange { get; set; }

    [CliOption("--protocol")]
    public int? Protocol { get; set; }

    [CliOption("--destination-cidr-block")]
    public string? DestinationCidrBlock { get; set; }

    [CliOption("--source-cidr-block")]
    public string? SourceCidrBlock { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--remove-fields")]
    public string[]? RemoveFields { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}