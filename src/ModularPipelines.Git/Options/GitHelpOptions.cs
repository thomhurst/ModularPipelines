using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("help")]
[ExcludeFromCodeCoverage]
public record GitHelpOptions : GitOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--no-external-commands")]
    public virtual bool? NoExternalCommands { get; set; }

    [CliFlag("--no-aliases")]
    public virtual bool? NoAliases { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--config")]
    public virtual bool? Config { get; set; }

    [CliFlag("--guides")]
    public virtual bool? Guides { get; set; }

    [CliFlag("--user-interfaces")]
    public virtual bool? UserInterfaces { get; set; }

    [CliFlag("--developer-interfaces")]
    public virtual bool? DeveloperInterfaces { get; set; }

    [CliFlag("--info")]
    public virtual bool? Info { get; set; }

    [CliFlag("--man")]
    public virtual bool? Man { get; set; }

    [CliFlag("--web")]
    public virtual bool? Web { get; set; }
}