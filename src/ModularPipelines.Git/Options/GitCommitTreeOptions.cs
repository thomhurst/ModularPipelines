using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("commit-tree")]
[ExcludeFromCodeCoverage]
public record GitCommitTreeOptions : GitOptions
{
    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }
}