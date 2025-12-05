using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("remote")]
[ExcludeFromCodeCoverage]
public record GitRemoteOptions : GitOptions
{
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }
}