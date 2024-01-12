using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "run")]
public record GcloudBuildsTriggersRunOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--substitutions")]
    public IEnumerable<KeyValue>? Substitutions { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--sha")]
    public string? Sha { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }
}