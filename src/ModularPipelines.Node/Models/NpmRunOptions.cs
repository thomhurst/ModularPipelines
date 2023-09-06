using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[CommandPrecedingArguments("run")]
[ExcludeFromCodeCoverage]
public record NpmRunOptions([property: PositionalArgument] string Target) : NpmOptions;
