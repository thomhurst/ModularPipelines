using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quantum", "target", "set")]
public record AzQuantumTargetSetOptions(
[property: CliOption("--target-id")] string TargetId
) : AzOptions;