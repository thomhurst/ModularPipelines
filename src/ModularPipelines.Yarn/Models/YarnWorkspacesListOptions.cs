using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "list")]
public record YarnWorkspacesListOptions : YarnOptions
{
    [BooleanCommandSwitch("--since")]
    public virtual bool? Since { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [BooleanCommandSwitch("--no-private")]
    public virtual bool? NoPrivate { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }
}