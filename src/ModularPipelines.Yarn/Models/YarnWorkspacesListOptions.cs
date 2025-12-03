using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "list")]
public record YarnWorkspacesListOptions : YarnOptions
{
    [CliFlag("--since")]
    public virtual bool? Since { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--no-private")]
    public virtual bool? NoPrivate { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}