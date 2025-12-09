using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-policy", "delete")]
public record AzDataprotectionBackupPolicyDeleteOptions : AzOptions
{
    [CliOption("--backup-policy-name")]
    public string? BackupPolicyName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}