using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("outdated")]
public record NpmOutdatedOptions : NpmOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }
}