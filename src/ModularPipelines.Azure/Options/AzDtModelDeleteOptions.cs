using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "model", "delete")]
public record AzDtModelDeleteOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--dtmi")] string Dtmi
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

