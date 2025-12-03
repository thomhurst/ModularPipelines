using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("verify-pack")]
[ExcludeFromCodeCoverage]
public record GitVerifyPackOptions : GitOptions
{
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--stat-only")]
    public virtual bool? StatOnly { get; set; }
}