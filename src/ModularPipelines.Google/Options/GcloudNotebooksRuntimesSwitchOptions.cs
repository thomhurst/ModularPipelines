using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "runtimes", "switch")]
public record GcloudNotebooksRuntimesSwitchOptions(
[property: PositionalArgument] string Runtime,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CommandSwitch("--accelerator-type")]
    public string? AcceleratorType { get; set; }
}