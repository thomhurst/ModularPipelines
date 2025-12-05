using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-network-insights-path")]
public record AwsEc2CreateNetworkInsightsPathOptions(
[property: CliOption("--protocol")] string Protocol
) : AwsOptions
{
    [CliOption("--source-ip")]
    public string? SourceIp { get; set; }

    [CliOption("--destination-ip")]
    public string? DestinationIp { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--destination-port")]
    public int? DestinationPort { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--filter-at-source")]
    public string? FilterAtSource { get; set; }

    [CliOption("--filter-at-destination")]
    public string? FilterAtDestination { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}