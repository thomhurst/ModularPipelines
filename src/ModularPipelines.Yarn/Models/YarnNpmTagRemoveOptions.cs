using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "tag", "remove")]
public record YarnNpmTagRemoveOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Package,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Tag
) : YarnOptions;