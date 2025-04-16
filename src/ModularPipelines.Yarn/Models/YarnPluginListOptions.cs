using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("plugin", "list")]
public record YarnPluginListOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }
}