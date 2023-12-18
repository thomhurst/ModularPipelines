using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "list")]
public record YarnWorkspacesListOptions : YarnOptions
{
    [BooleanCommandSwitch("--since")]
    public bool? Since { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--no-private")]
    public bool? NoPrivate { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }
}