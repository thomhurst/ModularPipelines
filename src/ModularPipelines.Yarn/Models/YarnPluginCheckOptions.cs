using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("plugin", "check")]
public record YarnPluginCheckOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}