using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "migration", "show", "(storage-preview", "extension)")]
public record AzStorageAccountMigrationShowStoragePreviewExtensionOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--migration-name")]
    public string? MigrationName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}