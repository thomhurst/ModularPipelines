using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "target", "show")]
public record AzQuantumTargetShowOptions : AzOptions
{
    [CommandSwitch("--target-id")]
    public string? TargetId { get; set; }
}