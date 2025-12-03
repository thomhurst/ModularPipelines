using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "runtimes", "switch")]
public record GcloudNotebooksRuntimesSwitchOptions(
[property: CliArgument] string Runtime,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CliOption("--accelerator-type")]
    public string? AcceleratorType { get; set; }
}