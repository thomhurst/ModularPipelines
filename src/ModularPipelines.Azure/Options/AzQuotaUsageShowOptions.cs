using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quota", "usage", "show")]
public record AzQuotaUsageShowOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions;