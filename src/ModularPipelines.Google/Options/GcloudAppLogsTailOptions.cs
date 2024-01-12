using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "logs", "tail")]
public record GcloudAppLogsTailOptions : GcloudOptions
{
    [CommandSwitch("--level")]
    public string? Level { get; set; }

    [CommandSwitch("--logs")]
    public string[]? Logs { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}