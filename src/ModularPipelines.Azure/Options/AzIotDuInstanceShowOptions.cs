using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "instance", "show")]
public record AzIotDuInstanceShowOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}