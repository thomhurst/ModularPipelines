using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-network-insights-path")]
public record AwsEc2CreateNetworkInsightsPathOptions(
[property: CommandSwitch("--protocol")] string Protocol
) : AwsOptions
{
    [CommandSwitch("--source-ip")]
    public string? SourceIp { get; set; }

    [CommandSwitch("--destination-ip")]
    public string? DestinationIp { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--destination-port")]
    public int? DestinationPort { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--filter-at-source")]
    public string? FilterAtSource { get; set; }

    [CommandSwitch("--filter-at-destination")]
    public string? FilterAtDestination { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}