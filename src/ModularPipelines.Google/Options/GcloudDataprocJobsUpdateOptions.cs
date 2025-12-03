using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "update")]
public record GcloudDataprocJobsUpdateOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Region,
[property: CliOption("--update-labels")] IEnumerable<KeyValue> UpdateLabels,
[property: CliFlag("--clear-labels")] bool ClearLabels,
[property: CliOption("--remove-labels")] string[] RemoveLabels
) : GcloudOptions;