using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "endpoint", "create", "eventgrid")]
public record AzDtEndpointCreateEventgridOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--egt")] string Egt,
[property: CommandSwitch("--en")] string En
) : AzOptions
{
    [CommandSwitch("--deadletter-sas-uri")]
    public string? DeadletterSasUri { get; set; }

    [CommandSwitch("--deadletter-uri")]
    public string? DeadletterUri { get; set; }

    [CommandSwitch("--egg")]
    public string? Egg { get; set; }

    [CommandSwitch("--egs")]
    public string? Egs { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

