using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("plugin", "runtime")]
public record YarnPluginRuntimeOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }
}