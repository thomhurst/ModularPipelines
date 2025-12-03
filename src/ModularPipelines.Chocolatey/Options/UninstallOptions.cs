using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("uninstall")]
public record UninstallOptions(
    [property: CliArgument] string Package
) : ChocoOptions;