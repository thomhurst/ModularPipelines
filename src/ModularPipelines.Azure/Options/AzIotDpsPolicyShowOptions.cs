using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "policy", "show")]
public record AzIotDpsPolicyShowOptions(
[property: CommandSwitch("--dps-name")] string DpsName,
[property: CommandSwitch("--pn")] string Pn
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}