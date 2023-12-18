using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "diagnostics", "validate-properties")]
public record AzIotCentralDiagnosticsValidatePropertiesOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--minimum-severity")]
    public string? MinimumSeverity { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}

