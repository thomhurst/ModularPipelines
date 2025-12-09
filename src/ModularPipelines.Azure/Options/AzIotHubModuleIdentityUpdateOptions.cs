using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "module-identity", "update")]
public record AzIotHubModuleIdentityUpdateOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--module-id")] string ModuleId
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}