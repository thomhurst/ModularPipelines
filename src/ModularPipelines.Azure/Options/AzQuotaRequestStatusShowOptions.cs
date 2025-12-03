using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quota", "request", "status", "show")]
public record AzQuotaRequestStatusShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope
) : AzOptions;