using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "list")]
public record AzScvmmVmListOptions(
[property: CommandSwitch("--checkpoint-id")] string CheckpointId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}