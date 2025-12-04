using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("show-ref")]
[ExcludeFromCodeCoverage]
public record GitShowRefOptions : GitOptions
{
    [CliFlag("--head")]
    public virtual bool? Head { get; set; }

    [CliFlag("--heads")]
    public virtual bool? Heads { get; set; }

    [CliFlag("--tags")]
    public virtual bool? Tags { get; set; }

    [CliFlag("--dereference")]
    public virtual bool? Dereference { get; set; }

    [CliOption("--hash", Format = OptionFormat.EqualsSeparated)]
    public string? Hash { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public string? Abbrev { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--exclude-existing", Format = OptionFormat.EqualsSeparated)]
    public string? ExcludeExisting { get; set; }
}