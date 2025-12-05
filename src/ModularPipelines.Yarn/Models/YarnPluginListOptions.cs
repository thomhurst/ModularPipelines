using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("plugin", "list")]
public record YarnPluginListOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}