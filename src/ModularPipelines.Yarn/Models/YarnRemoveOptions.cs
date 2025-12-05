using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("remove")]
public record YarnRemoveOptions : YarnOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--mode")]
    public virtual string? Mode { get; set; }
}