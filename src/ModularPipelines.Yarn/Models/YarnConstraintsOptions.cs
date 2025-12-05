using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("constraints")]
public record YarnConstraintsOptions : YarnOptions
{
    [CliFlag("--fix")]
    public virtual bool? Fix { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}