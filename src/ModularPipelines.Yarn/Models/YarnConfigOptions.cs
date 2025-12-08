using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("config")]
public record YarnConfigOptions : YarnOptions
{
    [CliFlag("--no-defaults")]
    public virtual bool? NoDefaults { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}