using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "device-identity", "import")]
public record AzIotHubDeviceIdentityImportOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--ibc")]
    public string? Ibc { get; set; }

    [CliOption("--ibcu")]
    public string? Ibcu { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--input-storage-account")]
    public int? InputStorageAccount { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--obc")]
    public string? Obc { get; set; }

    [CliOption("--obcu")]
    public string? Obcu { get; set; }

    [CliOption("--osa")]
    public string? Osa { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}