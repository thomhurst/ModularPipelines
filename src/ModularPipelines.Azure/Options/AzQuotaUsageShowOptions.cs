using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quota", "usage", "show")]
public record AzQuotaUsageShowOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--scope")] string Scope
) : AzOptions;