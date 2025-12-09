using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("count-objects")]
[ExcludeFromCodeCoverage]
public record GitCountObjectsOptions : GitOptions
{
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--human-readable")]
    public virtual bool? HumanReadable { get; set; }
}