using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workflows", "deploy")]
public record GcloudWorkflowsDeployOptions(
[property: CliArgument] string Workflow,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--call-log-level")]
    public string? CallLogLevel { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}