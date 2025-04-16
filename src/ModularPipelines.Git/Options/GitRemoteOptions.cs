using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("remote")]
[ExcludeFromCodeCoverage]
public record GitRemoteOptions : GitOptions
{
    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }
}