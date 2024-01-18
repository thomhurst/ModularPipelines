using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run")]
public record NpmRunOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string ScriptName
) : NpmOptions;