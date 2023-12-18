using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("purview", "default-account", "set")]
public record AzPurviewDefaultAccountSetOptions(
[property: CommandSwitch("--scope-tenant-id")] string ScopeTenantId,
[property: CommandSwitch("--subscription-id")] string SubscriptionId
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--scope-type")]
    public string? ScopeType { get; set; }
}