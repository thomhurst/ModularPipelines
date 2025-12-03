using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quantum", "target", "clear")]
public record AzQuantumTargetClearOptions : AzOptions;