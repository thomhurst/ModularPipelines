using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("mergetool")]
public record GitMergetoolOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--tool")]
    public string? MergeTool { get; set; }

    [BooleanCommandSwitch("--tool-help")]
    public bool? ToolHelp { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [BooleanCommandSwitch("--prompt")]
    public bool? Prompt { get; set; }

    [BooleanCommandSwitch("--gui")]
    public bool? Gui { get; set; }

    [BooleanCommandSwitch("--no-gui")]
    public bool? NoGui { get; set; }

}