using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("plugin", "remove")]
public record YarnPluginRemoveOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Name
) : YarnOptions
{
}