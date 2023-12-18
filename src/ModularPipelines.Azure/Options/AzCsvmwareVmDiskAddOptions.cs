using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("csvmware", "vm", "disk", "add")]
public record AzCsvmwareVmDiskAddOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--controller")]
    public string? Controller { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }
}