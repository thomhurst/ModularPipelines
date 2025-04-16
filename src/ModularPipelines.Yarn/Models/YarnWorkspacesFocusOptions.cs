using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "focus")]
public record YarnWorkspacesFocusOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--production")]
    public virtual bool? Production { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }
}