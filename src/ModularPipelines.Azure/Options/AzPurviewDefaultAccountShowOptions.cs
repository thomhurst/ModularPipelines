using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("purview", "default-account", "show")]
public record AzPurviewDefaultAccountShowOptions(
[property: CliOption("--scope-tenant-id")] string ScopeTenantId,
[property: CliOption("--scope-type")] string ScopeType
) : AzOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }
}