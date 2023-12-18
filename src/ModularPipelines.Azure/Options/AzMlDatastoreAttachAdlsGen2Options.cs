using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "datastore", "attach-adls-gen2")]
public record AzMlDatastoreAttachAdlsGen2Options(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--client-secret")] string ClientSecret,
[property: CommandSwitch("--file-system")] string FileSystem,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--tenant-id")] string TenantId
) : AzOptions
{
    [CommandSwitch("--adlsgen2-account-resource-group")]
    public int? Adlsgen2AccountResourceGroup { get; set; }

    [CommandSwitch("--adlsgen2-account-subscription-id")]
    public int? Adlsgen2AccountSubscriptionId { get; set; }

    [CommandSwitch("--authority-url")]
    public string? AuthorityUrl { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--grant-workspace-msi-access")]
    public string? GrantWorkspaceMsiAccess { get; set; }

    [CommandSwitch("--include-secret")]
    public string? IncludeSecret { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-url")]
    public string? ResourceUrl { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}

