using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "run")]
public record GcloudBuildsTriggersRunOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--substitutions")]
    public IEnumerable<KeyValue>? Substitutions { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--sha")]
    public string? Sha { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }
}