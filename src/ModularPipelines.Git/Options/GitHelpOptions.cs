using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("help")]
[ExcludeFromCodeCoverage]
public record GitHelpOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--no-external-commands")]
    public virtual bool? NoExternalCommands { get; set; }

    [BooleanCommandSwitch("--no-aliases")]
    public virtual bool? NoAliases { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--config")]
    public virtual bool? Config { get; set; }

    [BooleanCommandSwitch("--guides")]
    public virtual bool? Guides { get; set; }

    [BooleanCommandSwitch("--user-interfaces")]
    public virtual bool? UserInterfaces { get; set; }

    [BooleanCommandSwitch("--developer-interfaces")]
    public virtual bool? DeveloperInterfaces { get; set; }

    [BooleanCommandSwitch("--info")]
    public virtual bool? Info { get; set; }

    [BooleanCommandSwitch("--man")]
    public virtual bool? Man { get; set; }

    [BooleanCommandSwitch("--web")]
    public virtual bool? Web { get; set; }
}