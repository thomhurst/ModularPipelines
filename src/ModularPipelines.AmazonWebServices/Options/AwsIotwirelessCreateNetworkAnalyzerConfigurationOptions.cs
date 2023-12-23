using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "create-network-analyzer-configuration")]
public record AwsIotwirelessCreateNetworkAnalyzerConfigurationOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--trace-content")]
    public string? TraceContent { get; set; }

    [CommandSwitch("--wireless-devices")]
    public string[]? WirelessDevices { get; set; }

    [CommandSwitch("--wireless-gateways")]
    public string[]? WirelessGateways { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--multicast-groups")]
    public string[]? MulticastGroups { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}