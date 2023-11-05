using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pack")]
public record PackOptions(
    [property: PositionalArgument] string Path
) : ChocoOptions;