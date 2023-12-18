using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "config", "soft-delete", "show")]
public record AzAcrConfigSoftDeleteShowOptions(
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--days")]
    public int? Days { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}