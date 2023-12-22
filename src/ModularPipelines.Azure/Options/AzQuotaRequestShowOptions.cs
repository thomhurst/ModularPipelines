using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quota", "request", "show")]
public record AzQuotaRequestShowOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions;