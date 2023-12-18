using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "state", "export")]
public record AzIotHubStateExportOptions(
[property: CommandSwitch("--state-file")] string StateFile
) : AzOptions
{
    [CommandSwitch("--aspects")]
    public string? Aspects { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [BooleanCommandSwitch("--replace")]
    public bool? Replace { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

