using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("mergetool")]
[ExcludeFromCodeCoverage]
public record GitMergetoolOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--tool")]
    public string? MergeTool { get; set; }

    [BooleanCommandSwitch("--tool-help")]
    public virtual bool? ToolHelp { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public virtual bool? NoPrompt { get; set; }

    [BooleanCommandSwitch("--prompt")]
    public virtual bool? Prompt { get; set; }

    [BooleanCommandSwitch("--gui")]
    public virtual bool? Gui { get; set; }

    [BooleanCommandSwitch("--no-gui")]
    public virtual bool? NoGui { get; set; }
}