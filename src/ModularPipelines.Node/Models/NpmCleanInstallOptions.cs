using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[CommandPrecedingArguments("ci")]
[ExcludeFromCodeCoverage]
public record NpmCleanInstallOptions([property: PositionalArgument] string Target) : NpmOptions
{
    [CommandEqualsSeparatorSwitch("--install-strategy")]
    public string? InstallStrategy { get; init; }

    [CommandEqualsSeparatorSwitch("--omit")]
    public IEnumerable<string>? Omit { get; init; }
}
