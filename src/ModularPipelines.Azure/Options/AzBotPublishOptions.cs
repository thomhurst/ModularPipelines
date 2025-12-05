using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bot", "publish")]
public record AzBotPublishOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--code-dir")]
    public string? CodeDir { get; set; }

    [CliFlag("--keep-node-modules")]
    public bool? KeepNodeModules { get; set; }

    [CliOption("--proj-file-path")]
    public string? ProjFilePath { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}