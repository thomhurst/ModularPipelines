using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "device-identity", "list")]
public record AzIotHubDeviceIdentityListOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliFlag("--edge-enabled")]
    public bool? EdgeEnabled { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}