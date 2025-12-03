using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("fsck")]
[ExcludeFromCodeCoverage]
public record GitFsckOptions : GitOptions
{
    [CliFlag("--unreachable")]
    public virtual bool? Unreachable { get; set; }

    [CliFlag("--no-dangling")]
    public virtual bool? NoDangling { get; set; }

    [CliFlag("--dangling")]
    public virtual bool? Dangling { get; set; }

    [CliFlag("--root")]
    public virtual bool? Root { get; set; }

    [CliFlag("--tags")]
    public virtual bool? Tags { get; set; }

    [CliFlag("--cache")]
    public virtual bool? Cache { get; set; }

    [CliFlag("--no-reflogs")]
    public virtual bool? NoReflogs { get; set; }

    [CliFlag("--full")]
    public virtual bool? Full { get; set; }

    [CliFlag("--connectivity-only")]
    public virtual bool? ConnectivityOnly { get; set; }

    [CliFlag("--strict")]
    public virtual bool? Strict { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--lost-found")]
    public virtual bool? LostFound { get; set; }

    [CliFlag("--name-objects")]
    public virtual bool? NameObjects { get; set; }

    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }
}