using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "config", "init")]
public record AzArcdataDcConfigInitOptions(
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--patch-file")] string PatchFile,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }
}