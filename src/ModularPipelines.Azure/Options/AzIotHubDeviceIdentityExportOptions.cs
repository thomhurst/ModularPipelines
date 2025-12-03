using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "device-identity", "export")]
public record AzIotHubDeviceIdentityExportOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--bc")]
    public string? Bc { get; set; }

    [CliOption("--bcu")]
    public string? Bcu { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliFlag("--ik")]
    public bool? Ik { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sa")]
    public string? Sa { get; set; }
}