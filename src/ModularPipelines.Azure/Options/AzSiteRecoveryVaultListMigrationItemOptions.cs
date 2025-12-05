using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "vault", "list-migration-item")]
public record AzSiteRecoveryVaultListMigrationItemOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--take-token")]
    public string? TakeToken { get; set; }
}