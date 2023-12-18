using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "tag", "list")]
public record YarnNpmTagListOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Package { get; set; }
}