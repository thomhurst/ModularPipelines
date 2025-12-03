using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "device", "c2d-message", "receive")]
public record AzIotDeviceC2dMessageReceiveOptions(
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliFlag("--abandon")]
    public bool? Abandon { get; set; }

    [CliFlag("--complete")]
    public bool? Complete { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--lock-timeout")]
    public string? LockTimeout { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliFlag("--reject")]
    public bool? Reject { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}