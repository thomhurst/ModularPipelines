using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "update")]
public record GcloudNotebooksInstancesUpdateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location,
[property: CliOption("--accelerator-core-count")] string AcceleratorCoreCount,
[property: CliOption("--accelerator-type")] string AcceleratorType,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels,
[property: CliOption("--machine-type")] string MachineType
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}