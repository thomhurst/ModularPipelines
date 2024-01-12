using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "jobs", "update")]
public record GcloudDataprocJobsUpdateOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--update-labels")] IEnumerable<KeyValue> UpdateLabels,
[property: BooleanCommandSwitch("--clear-labels")] bool ClearLabels,
[property: CommandSwitch("--remove-labels")] string[] RemoveLabels
) : GcloudOptions;