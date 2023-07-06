using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[CommandPrecedingArguments("run")]
public record NpmRunOptions(string Target) : NpmOptions;