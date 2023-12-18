using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "target", "show")]
public record AzQuantumTargetShowOptions : AzOptions
{
    [CommandSwitch("--target-id")]
    public string? TargetId { get; set; }
}

