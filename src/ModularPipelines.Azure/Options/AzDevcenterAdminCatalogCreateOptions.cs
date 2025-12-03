using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "catalog", "create")]
public record AzDevcenterAdminCatalogCreateOptions(
[property: CliOption("--catalog-name")] string CatalogName,
[property: CliOption("--dev-center")] string DevCenter,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ado-git")]
    public string? AdoGit { get; set; }

    [CliOption("--git-hub")]
    public string? GitHub { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sync-type")]
    public string? SyncType { get; set; }
}