using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud", "unregister")]
public record AzCloudUnregisterOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--cloud-config")]
    public string? CloudConfig { get; set; }

    [CommandSwitch("--endpoint-active-directory")]
    public string? EndpointActiveDirectory { get; set; }

    [CommandSwitch("--endpoint-active-directory-data-lake-resource-id")]
    public string? EndpointActiveDirectoryDataLakeResourceId { get; set; }

    [CommandSwitch("--endpoint-active-directory-graph-resource-id")]
    public string? EndpointActiveDirectoryGraphResourceId { get; set; }

    [CommandSwitch("--endpoint-active-directory-resource-id")]
    public string? EndpointActiveDirectoryResourceId { get; set; }

    [CommandSwitch("--endpoint-gallery")]
    public string? EndpointGallery { get; set; }

    [CommandSwitch("--endpoint-management")]
    public string? EndpointManagement { get; set; }

    [CommandSwitch("--endpoint-resource-manager")]
    public string? EndpointResourceManager { get; set; }

    [CommandSwitch("--endpoint-sql-management")]
    public string? EndpointSqlManagement { get; set; }

    [CommandSwitch("--endpoint-vm-image-alias-doc")]
    public string? EndpointVmImageAliasDoc { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [CommandSwitch("--suffix-acr-login-server-endpoint")]
    public string? SuffixAcrLoginServerEndpoint { get; set; }

    [CommandSwitch("--suffix-azure-datalake-analytics-catalog-and-job-endpoint")]
    public string? SuffixAzureDatalakeAnalyticsCatalogAndJobEndpoint { get; set; }

    [CommandSwitch("--suffix-azure-datalake-store-file-system-endpoint")]
    public string? SuffixAzureDatalakeStoreFileSystemEndpoint { get; set; }

    [CommandSwitch("--suffix-keyvault-dns")]
    public string? SuffixKeyvaultDns { get; set; }

    [CommandSwitch("--suffix-sql-server-hostname")]
    public string? SuffixSqlServerHostname { get; set; }

    [CommandSwitch("--suffix-storage-endpoint")]
    public string? SuffixStorageEndpoint { get; set; }
}