using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quantum", "target", "show")]
public record AzQuantumTargetShowOptions : AzOptions
{
    [CliOption("--target-id")]
    public string? TargetId { get; set; }
}