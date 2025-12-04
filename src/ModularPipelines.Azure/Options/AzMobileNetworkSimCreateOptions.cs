using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "sim", "create")]
public record AzMobileNetworkSimCreateOptions(
[property: CliOption("--international-msi")] string InternationalMsi,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sim-group-name")] string SimGroupName
) : AzOptions
{
    [CliOption("--authentication-key")]
    public string? AuthenticationKey { get; set; }

    [CliOption("--device-type")]
    public string? DeviceType { get; set; }

    [CliOption("--icc-id")]
    public string? IccId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--operator-key-code")]
    public string? OperatorKeyCode { get; set; }

    [CliOption("--sim-policy")]
    public string? SimPolicy { get; set; }

    [CliOption("--static-ip-config")]
    public string? StaticIpConfig { get; set; }
}