using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quota", "show")]
public record AzQuotaShowOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--scope")] string Scope
) : AzOptions;