using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("version", "check")]
public record YarnVersionCheckOptions : YarnOptions
{
    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }
}