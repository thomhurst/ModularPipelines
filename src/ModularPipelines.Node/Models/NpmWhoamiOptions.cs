using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("whoami")]
public record NpmWhoamiOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }
}