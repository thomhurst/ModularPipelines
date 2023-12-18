using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "config", "retention", "update")]
public record AzAcrConfigRetentionUpdateOptions(
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--days")]
    public int? Days { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}