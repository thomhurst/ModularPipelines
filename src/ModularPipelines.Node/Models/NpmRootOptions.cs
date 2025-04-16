using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("root")]
public record NpmRootOptions : NpmOptions
{
    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }
}