using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "config", "soft-delete", "update")]
public record AzAcrConfigSoftDeleteUpdateOptions(
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