using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "catalog", "create")]
public record AzDevcenterAdminCatalogCreateOptions(
[property: CommandSwitch("--catalog-name")] string CatalogName,
[property: CommandSwitch("--dev-center")] string DevCenter,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ado-git")]
    public string? AdoGit { get; set; }

    [CommandSwitch("--git-hub")]
    public string? GitHub { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sync-type")]
    public string? SyncType { get; set; }
}