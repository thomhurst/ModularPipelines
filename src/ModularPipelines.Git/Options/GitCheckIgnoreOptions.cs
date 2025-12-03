using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("check-ignore")]
[ExcludeFromCodeCoverage]
public record GitCheckIgnoreOptions : GitOptions
{
    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--no-index")]
    public virtual bool? NoIndex { get; set; }
}