using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bot", "publish")]
public record AzBotPublishOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--code-dir")]
    public string? CodeDir { get; set; }

    [BooleanCommandSwitch("--keep-node-modules")]
    public bool? KeepNodeModules { get; set; }

    [CommandSwitch("--proj-file-path")]
    public string? ProjFilePath { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}

