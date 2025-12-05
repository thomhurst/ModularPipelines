using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-traffic-mirror-filter-rule")]
public record AwsEc2CreateTrafficMirrorFilterRuleOptions(
[property: CliOption("--traffic-mirror-filter-id")] string TrafficMirrorFilterId,
[property: CliOption("--traffic-direction")] string TrafficDirection,
[property: CliOption("--rule-number")] int RuleNumber,
[property: CliOption("--rule-action")] string RuleAction,
[property: CliOption("--destination-cidr-block")] string DestinationCidrBlock,
[property: CliOption("--source-cidr-block")] string SourceCidrBlock
) : AwsOptions
{
    [CliOption("--destination-port-range")]
    public string? DestinationPortRange { get; set; }

    [CliOption("--source-port-range")]
    public string? SourcePortRange { get; set; }

    [CliOption("--protocol")]
    public int? Protocol { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}