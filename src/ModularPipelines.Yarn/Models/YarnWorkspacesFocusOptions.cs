using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "focus")]
public record YarnWorkspacesFocusOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--production")]
    public virtual bool? Production { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }
}