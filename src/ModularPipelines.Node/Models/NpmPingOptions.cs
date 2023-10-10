using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ping")]
public record NpmPingOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

}