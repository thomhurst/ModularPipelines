using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud", "register")]
public record AzCloudRegisterOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--cloud-config")]
    public string? CloudConfig { get; set; }

    [CliOption("--endpoint-active-directory")]
    public string? EndpointActiveDirectory { get; set; }

    [CliOption("--endpoint-active-directory-data-lake-resource-id")]
    public string? EndpointActiveDirectoryDataLakeResourceId { get; set; }

    [CliOption("--endpoint-active-directory-graph-resource-id")]
    public string? EndpointActiveDirectoryGraphResourceId { get; set; }

    [CliOption("--endpoint-active-directory-resource-id")]
    public string? EndpointActiveDirectoryResourceId { get; set; }

    [CliOption("--endpoint-gallery")]
    public string? EndpointGallery { get; set; }

    [CliOption("--endpoint-management")]
    public string? EndpointManagement { get; set; }

    [CliOption("--endpoint-resource-manager")]
    public string? EndpointResourceManager { get; set; }

    [CliOption("--endpoint-sql-management")]
    public string? EndpointSqlManagement { get; set; }

    [CliOption("--endpoint-vm-image-alias-doc")]
    public string? EndpointVmImageAliasDoc { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }

    [CliOption("--suffix-acr-login-server-endpoint")]
    public string? SuffixAcrLoginServerEndpoint { get; set; }

    [CliOption("--suffix-azure-datalake-analytics-catalog-and-job-endpoint")]
    public string? SuffixAzureDatalakeAnalyticsCatalogAndJobEndpoint { get; set; }

    [CliOption("--suffix-azure-datalake-store-file-system-endpoint")]
    public string? SuffixAzureDatalakeStoreFileSystemEndpoint { get; set; }

    [CliOption("--suffix-keyvault-dns")]
    public string? SuffixKeyvaultDns { get; set; }

    [CliOption("--suffix-sql-server-hostname")]
    public string? SuffixSqlServerHostname { get; set; }

    [CliOption("--suffix-storage-endpoint")]
    public string? SuffixStorageEndpoint { get; set; }
}