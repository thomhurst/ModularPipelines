using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "model", "update")]
public record AzDtModelUpdateOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--dtmi")] string Dtmi
) : AzOptions
{
    [BooleanCommandSwitch("--decommission")]
    public bool? Decommission { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}