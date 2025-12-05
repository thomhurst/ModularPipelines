using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "execute")]
public record GcloudWorkflowsExecuteOptions(
[property: CliArgument] string Workflow,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--call-log-level")]
    public string? CallLogLevel { get; set; }

    [CliOption("--data")]
    public string? Data { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}