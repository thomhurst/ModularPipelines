using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "message-route", "test")]
public record AzIotHubMessageRouteTestOptions(
[property: CommandSwitch("--hub-name")] string HubName
) : AzOptions
{
    [CommandSwitch("--ap")]
    public string? Ap { get; set; }

    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rn")]
    public string? Rn { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--sp")]
    public string? Sp { get; set; }
}

