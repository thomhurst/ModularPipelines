using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quota", "show")]
public record AzQuotaShowOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions;