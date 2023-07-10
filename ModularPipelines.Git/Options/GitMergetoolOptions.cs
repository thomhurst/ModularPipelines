using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("mergetool")]
public record GitMergetoolOptions : GitOptions
{
    [CommandLongSwitch("tool")]
    public string? Tool { get; set; }

    [BooleanCommandSwitch("tool-help")]
    public bool? ToolHelp { get; set; }

    [BooleanCommandSwitch("no-prompt")]
    public bool? NoPrompt { get; set; }

    [BooleanCommandSwitch("prompt")]
    public bool? Prompt { get; set; }

    [BooleanCommandSwitch("gui")]
    public bool? Gui { get; set; }

    [BooleanCommandSwitch("no-gui")]
    public bool? NoGui { get; set; }

}