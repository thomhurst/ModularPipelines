using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("purview", "default-account", "set")]
public record AzPurviewDefaultAccountSetOptions(
[property: CliOption("--scope-tenant-id")] string ScopeTenantId,
[property: CliOption("--subscription-id")] string SubscriptionId
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--scope-type")]
    public string? ScopeType { get; set; }
}