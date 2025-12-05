using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "update")]
public record GcloudSccFindingsUpdateOptions(
[property: CliArgument] string Finding,
[property: CliArgument] string Organization,
[property: CliArgument] string Source
) : GcloudOptions
{
    [CliOption("--event-time")]
    public string? EventTime { get; set; }

    [CliOption("--external-uri")]
    public string? ExternalUri { get; set; }

    [CliOption("--source-properties")]
    public IEnumerable<KeyValue>? SourceProperties { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }
}