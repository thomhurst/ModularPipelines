using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("adp", "workspace", "create")]
public record AzAdpWorkspaceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--data-location")]
    public string? DataLocation { get; set; }

    [CliOption("--direct-read-access")]
    public string? DirectReadAccess { get; set; }

    [CliOption("--domain-name-scope")]
    public string? DomainNameScope { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CliOption("--storage-account-count")]
    public int? StorageAccountCount { get; set; }

    [CliOption("--storage-sku")]
    public string? StorageSku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}