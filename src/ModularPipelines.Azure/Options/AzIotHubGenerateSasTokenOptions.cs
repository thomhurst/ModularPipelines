using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "generate-sas-token")]
public record AzIotHubGenerateSasTokenOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--device-id")]
    public string? DeviceId { get; set; }

    [CliOption("--du")]
    public string? Du { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--key-type")]
    public string? KeyType { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--module-id")]
    public string? ModuleId { get; set; }

    [CliOption("--pn")]
    public string? Pn { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}