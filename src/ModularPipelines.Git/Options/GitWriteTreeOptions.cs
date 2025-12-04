using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("write-tree")]
[ExcludeFromCodeCoverage]
public record GitWriteTreeOptions : GitOptions
{
    [CliFlag("--missing-ok")]
    public virtual bool? MissingOk { get; set; }

    [CliOption("--prefix", Format = OptionFormat.EqualsSeparated)]
    public string? Prefix { get; set; }
}