using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("plugin", "runtime")]
public record YarnPluginRuntimeOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }
}