using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("exec")]
public record YarnExecOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string CommandName
) : YarnOptions
{
}