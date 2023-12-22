using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("uninstall")]
public record UninstallOptions(
    [property: PositionalArgument] string Package
) : ChocoOptions;