using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "workspace", "clear")]
public record AzQuantumWorkspaceClearOptions : AzOptions;