using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pack")]
public record PackOptions(
    [property: CliArgument] string Path
) : ChocoOptions;