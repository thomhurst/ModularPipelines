using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "device-identity", "children", "remove")]
public record AzIotHubDeviceIdentityChildrenRemoveOptions(
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--child-list")]
    public string? ChildList { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliFlag("--remove-all")]
    public bool? RemoveAll { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}