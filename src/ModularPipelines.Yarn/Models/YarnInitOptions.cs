using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("init")]
public record YarnInitOptions : YarnOptions
{
    [CliFlag("--private")]
    public virtual bool? Private { get; set; }

    [CliFlag("--workspace")]
    public virtual bool? Workspace { get; set; }

    [CliFlag("--install")]
    public virtual bool? Install { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }
}