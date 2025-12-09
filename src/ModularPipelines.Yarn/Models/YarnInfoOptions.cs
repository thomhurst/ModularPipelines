using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("info")]
public record YarnInfoOptions : YarnOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--extra")]
    public virtual string? Extra { get; set; }

    [CliFlag("--cache")]
    public virtual bool? Cache { get; set; }

    [CliFlag("--dependents")]
    public virtual bool? Dependents { get; set; }

    [CliFlag("--manifest")]
    public virtual bool? Manifest { get; set; }

    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [CliFlag("--virtuals")]
    public virtual bool? Virtuals { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}