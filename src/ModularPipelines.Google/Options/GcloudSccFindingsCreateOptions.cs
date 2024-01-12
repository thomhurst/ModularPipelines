using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "findings", "create")]
public record GcloudSccFindingsCreateOptions(
[property: PositionalArgument] string Finding,
[property: PositionalArgument] string Organization,
[property: PositionalArgument] string Source,
[property: CommandSwitch("--category")] string Category,
[property: CommandSwitch("--event-time")] string EventTime,
[property: CommandSwitch("--resource-name")] string ResourceName
) : GcloudOptions
{
    [CommandSwitch("--external-uri")]
    public string? ExternalUri { get; set; }

    [CommandSwitch("--source-properties")]
    public IEnumerable<KeyValue>? SourceProperties { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}