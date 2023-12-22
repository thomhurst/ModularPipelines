using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "target", "set")]
public record AzQuantumTargetSetOptions(
[property: CommandSwitch("--target-id")] string TargetId
) : AzOptions;