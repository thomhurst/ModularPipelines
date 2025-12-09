using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "module-twin", "replace")]
public record AzIotHubModuleTwinReplaceOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--json")] string Json,
[property: CliOption("--module-id")] string ModuleId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}