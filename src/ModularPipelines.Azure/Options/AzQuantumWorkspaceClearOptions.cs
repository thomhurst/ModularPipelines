using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quantum", "workspace", "clear")]
public record AzQuantumWorkspaceClearOptions : AzOptions;