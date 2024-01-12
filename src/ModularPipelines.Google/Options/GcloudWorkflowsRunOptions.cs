using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workflows", "run")]
public record GcloudWorkflowsRunOptions(
[property: PositionalArgument] string Workflow,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--call-log-level")]
    public string? CallLogLevel { get; set; }

    [CommandSwitch("--data")]
    public string? Data { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}