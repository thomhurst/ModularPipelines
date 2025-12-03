using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "create")]
public record GcloudSccFindingsCreateOptions(
[property: CliArgument] string Finding,
[property: CliArgument] string Organization,
[property: CliArgument] string Source,
[property: CliOption("--category")] string Category,
[property: CliOption("--event-time")] string EventTime,
[property: CliOption("--resource-name")] string ResourceName
) : GcloudOptions
{
    [CliOption("--external-uri")]
    public string? ExternalUri { get; set; }

    [CliOption("--source-properties")]
    public IEnumerable<KeyValue>? SourceProperties { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}