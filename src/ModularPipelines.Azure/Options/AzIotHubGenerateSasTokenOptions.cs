using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "generate-sas-token")]
public record AzIotHubGenerateSasTokenOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--method-name")] string MethodName
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--du")]
    public string? Du { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--key-type")]
    public string? KeyType { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--module-id")]
    public string? ModuleId { get; set; }

    [CommandSwitch("--pn")]
    public string? Pn { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}