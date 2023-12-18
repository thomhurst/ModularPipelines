using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "route", "delete")]
public record AzDtRouteDeleteOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--rn")] string Rn
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}