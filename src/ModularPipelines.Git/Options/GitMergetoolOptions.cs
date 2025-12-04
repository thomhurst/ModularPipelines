using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("mergetool")]
[ExcludeFromCodeCoverage]
public record GitMergetoolOptions : GitOptions
{
    [CliOption("--tool", Format = OptionFormat.EqualsSeparated)]
    public string? MergeTool { get; set; }

    [CliFlag("--tool-help")]
    public virtual bool? ToolHelp { get; set; }

    [CliFlag("--no-prompt")]
    public virtual bool? NoPrompt { get; set; }

    [CliFlag("--prompt")]
    public virtual bool? Prompt { get; set; }

    [CliFlag("--gui")]
    public virtual bool? Gui { get; set; }

    [CliFlag("--no-gui")]
    public virtual bool? NoGui { get; set; }
}