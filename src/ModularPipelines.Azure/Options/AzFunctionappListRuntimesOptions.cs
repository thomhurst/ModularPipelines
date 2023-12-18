using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "list-runtimes")]
public record AzFunctionappListRuntimesOptions : AzOptions
{
    [CommandSwitch("--os")]
    public string? Os { get; set; }
}