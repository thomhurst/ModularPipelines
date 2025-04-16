using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("whoami")]
public record NpmWhoamiOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }
}