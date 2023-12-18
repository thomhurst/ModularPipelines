using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "datastore", "attach-sqldb")]
public record AzMlDatastoreAttachSqldbOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--server-name")] string ServerName
) : AzOptions
{
    [CommandSwitch("--authority-url")]
    public string? AuthorityUrl { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--grant-workspace-msi-access")]
    public string? GrantWorkspaceMsiAccess { get; set; }

    [CommandSwitch("--include-secret")]
    public string? IncludeSecret { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-url")]
    public string? ResourceUrl { get; set; }

    [CommandSwitch("--sql-resource-group")]
    public string? SqlResourceGroup { get; set; }

    [CommandSwitch("--sql-subscription-id")]
    public string? SqlSubscriptionId { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}

