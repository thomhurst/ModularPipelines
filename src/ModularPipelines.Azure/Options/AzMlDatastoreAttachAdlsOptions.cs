using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "datastore", "attach-adls")]
public record AzMlDatastoreAttachAdlsOptions(
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--client-secret")] string ClientSecret,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--store-name")] string StoreName,
[property: CommandSwitch("--tenant-id")] string TenantId
) : AzOptions
{
    [CommandSwitch("--adls-resource-group")]
    public string? AdlsResourceGroup { get; set; }

    [CommandSwitch("--adls-subscription-id")]
    public string? AdlsSubscriptionId { get; set; }

    [CommandSwitch("--authority-url")]
    public string? AuthorityUrl { get; set; }

    [CommandSwitch("--grant-workspace-msi-access")]
    public string? GrantWorkspaceMsiAccess { get; set; }

    [CommandSwitch("--include-secret")]
    public string? IncludeSecret { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-url")]
    public string? ResourceUrl { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}