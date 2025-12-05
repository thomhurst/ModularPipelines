using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("rm")]
[ExcludeFromCodeCoverage]
public record GitRmOptions : GitOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--cached")]
    public virtual bool? Cached { get; set; }

    [CliFlag("--ignore-unmatch")]
    public virtual bool? IgnoreUnmatch { get; set; }

    [CliFlag("--sparse")]
    public virtual bool? Sparse { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--pathspec-from-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? PathspecFromFile { get; set; }

    [CliFlag("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }
}