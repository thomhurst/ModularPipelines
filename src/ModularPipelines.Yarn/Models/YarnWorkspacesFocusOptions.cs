using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "focus")]
public record YarnWorkspacesFocusOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--production")]
    public bool? Production { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}