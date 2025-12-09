using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "datastore", "attach-adls")]
public record AzMlDatastoreAttachAdlsOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--client-secret")] string ClientSecret,
[property: CliOption("--name")] string Name,
[property: CliOption("--store-name")] string StoreName,
[property: CliOption("--tenant-id")] string TenantId
) : AzOptions
{
    [CliOption("--adls-resource-group")]
    public string? AdlsResourceGroup { get; set; }

    [CliOption("--adls-subscription-id")]
    public string? AdlsSubscriptionId { get; set; }

    [CliOption("--authority-url")]
    public string? AuthorityUrl { get; set; }

    [CliOption("--grant-workspace-msi-access")]
    public string? GrantWorkspaceMsiAccess { get; set; }

    [CliOption("--include-secret")]
    public string? IncludeSecret { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-url")]
    public string? ResourceUrl { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}