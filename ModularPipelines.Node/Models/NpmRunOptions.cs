using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[CommandPrecedingArguments("run")]
public record NpmRunOptions([property: PositionalArgument] string Target) : NpmOptions;
