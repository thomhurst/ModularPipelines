using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "route", "create")]
public record AzDtRouteCreateOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--rn")] string Rn
) : AzOptions
{
    [BooleanCommandSwitch("--filter")]
    public bool? Filter { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}