using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "datastore", "attach-sqldb")]
public record AzMlDatastoreAttachSqldbOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--server-name")] string ServerName
) : AzOptions
{
    [CliOption("--authority-url")]
    public string? AuthorityUrl { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--grant-workspace-msi-access")]
    public string? GrantWorkspaceMsiAccess { get; set; }

    [CliOption("--include-secret")]
    public string? IncludeSecret { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-url")]
    public string? ResourceUrl { get; set; }

    [CliOption("--sql-resource-group")]
    public string? SqlResourceGroup { get; set; }

    [CliOption("--sql-subscription-id")]
    public string? SqlSubscriptionId { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}