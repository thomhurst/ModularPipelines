using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "model", "show")]
public record AzDtModelShowOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--dtmi")] string Dtmi
) : AzOptions
{
    [BooleanCommandSwitch("--def")]
    public bool? Def { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

