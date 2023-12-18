using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("prefix")]
public record NpmPrefixOptions : NpmOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }
}