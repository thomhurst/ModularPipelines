using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("purview", "default-account", "remove")]
public record AzPurviewDefaultAccountRemoveOptions(
[property: CommandSwitch("--scope-tenant-id")] string ScopeTenantId,
[property: CommandSwitch("--scope-type")] string ScopeType
) : AzOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}