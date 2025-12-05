using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "create-network-analyzer-configuration")]
public record AwsIotwirelessCreateNetworkAnalyzerConfigurationOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--trace-content")]
    public string? TraceContent { get; set; }

    [CliOption("--wireless-devices")]
    public string[]? WirelessDevices { get; set; }

    [CliOption("--wireless-gateways")]
    public string[]? WirelessGateways { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--multicast-groups")]
    public string[]? MulticastGroups { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}