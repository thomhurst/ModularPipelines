using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "instances", "update")]
public record GcloudNotebooksInstancesUpdateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--accelerator-core-count")] string AcceleratorCoreCount,
[property: CommandSwitch("--accelerator-type")] string AcceleratorType,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels,
[property: CommandSwitch("--machine-type")] string MachineType
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}