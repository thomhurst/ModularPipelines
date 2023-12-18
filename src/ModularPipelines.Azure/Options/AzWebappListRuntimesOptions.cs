using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "list-runtimes")]
public record AzWebappListRuntimesOptions : AzOptions
{
    [CommandSwitch("--os")]
    public string? Os { get; set; }
}