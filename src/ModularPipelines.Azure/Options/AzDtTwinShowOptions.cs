using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "show")]
public record AzDtTwinShowOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--twin-id")] string TwinId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}