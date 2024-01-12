using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "findings", "update")]
public record GcloudSccFindingsUpdateOptions(
[property: PositionalArgument] string Finding,
[property: PositionalArgument] string Organization,
[property: PositionalArgument] string Source
) : GcloudOptions
{
    [CommandSwitch("--event-time")]
    public string? EventTime { get; set; }

    [CommandSwitch("--external-uri")]
    public string? ExternalUri { get; set; }

    [CommandSwitch("--source-properties")]
    public IEnumerable<KeyValue>? SourceProperties { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--update-mask")]
    public string? UpdateMask { get; set; }
}