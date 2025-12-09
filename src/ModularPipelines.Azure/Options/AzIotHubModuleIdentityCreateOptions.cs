using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "module-identity", "create")]
public record AzIotHubModuleIdentityCreateOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--module-id")] string ModuleId
) : AzOptions
{
    [CliOption("--am")]
    public string? Am { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--od")]
    public string? Od { get; set; }

    [CliOption("--pk")]
    public string? Pk { get; set; }

    [CliOption("--primary-thumbprint")]
    public string? PrimaryThumbprint { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CliOption("--secondary-thumbprint")]
    public string? SecondaryThumbprint { get; set; }

    [CliOption("--valid-days")]
    public string? ValidDays { get; set; }
}