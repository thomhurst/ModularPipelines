using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "tasks", "run")]
public record GcloudDataplexTasksRunOptions(
[property: CliArgument] string Task,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string ExecutionSpecArgs
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}