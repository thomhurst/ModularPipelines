using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("purview", "default-account", "remove")]
public record AzPurviewDefaultAccountRemoveOptions(
[property: CliOption("--scope-tenant-id")] string ScopeTenantId,
[property: CliOption("--scope-type")] string ScopeType
) : AzOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }
}