using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace")]
public record YarnWorkspaceOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string WorkspaceName,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string CommandName
) : YarnOptions;