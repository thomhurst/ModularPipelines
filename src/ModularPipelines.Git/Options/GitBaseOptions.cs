using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[ExcludeFromCodeCoverage]
public record GitBaseOptions : GitOptions
{
    [CliFlag("--version")]
    public virtual bool? Version { get; set; }

    [CliFlag("--html-path")]
    public virtual bool? HtmlPath { get; set; }

    [CliFlag("--man-path")]
    public virtual bool? ManPath { get; set; }

    [CliFlag("--info-path")]
    public virtual bool? InfoPath { get; set; }

    [CliFlag("--list-cmds")]
    public virtual bool? ListCmds { get; set; }
}
