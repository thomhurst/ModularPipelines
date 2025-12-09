using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("quota", "request", "show")]
public record AzQuotaRequestShowOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--scope")] string Scope
) : AzOptions;