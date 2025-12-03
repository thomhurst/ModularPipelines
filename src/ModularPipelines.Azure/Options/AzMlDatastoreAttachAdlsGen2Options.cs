using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "datastore", "attach-adls-gen2")]
public record AzMlDatastoreAttachAdlsGen2Options(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--client-secret")] string ClientSecret,
[property: CliOption("--file-system")] string FileSystem,
[property: CliOption("--name")] string Name,
[property: CliOption("--tenant-id")] string TenantId
) : AzOptions
{
    [CliOption("--adlsgen2-account-resource-group")]
    public int? Adlsgen2AccountResourceGroup { get; set; }

    [CliOption("--adlsgen2-account-subscription-id")]
    public int? Adlsgen2AccountSubscriptionId { get; set; }

    [CliOption("--authority-url")]
    public string? AuthorityUrl { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--grant-workspace-msi-access")]
    public string? GrantWorkspaceMsiAccess { get; set; }

    [CliOption("--include-secret")]
    public string? IncludeSecret { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-url")]
    public string? ResourceUrl { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}