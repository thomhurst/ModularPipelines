using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shrinkwrap")]
public record NpmShrinkwrapOptions : NpmOptions
{
}