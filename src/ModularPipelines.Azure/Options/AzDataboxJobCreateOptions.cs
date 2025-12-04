using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databox", "job", "create")]
public record AzDataboxJobCreateOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku,
[property: CliOption("--transfer-type")] string TransferType
) : AzOptions
{
    [CliOption("--city")]
    public string? City { get; set; }

    [CliOption("--company-name")]
    public string? CompanyName { get; set; }

    [CliOption("--contact-name")]
    public string? ContactName { get; set; }

    [CliOption("--country")]
    public int? Country { get; set; }

    [CliOption("--data-box-customer-disk")]
    public string? DataBoxCustomerDisk { get; set; }

    [CliOption("--email-list")]
    public string? EmailList { get; set; }

    [CliOption("--expected-data-size")]
    public string? ExpectedDataSize { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--kek-identity")]
    public string? KekIdentity { get; set; }

    [CliOption("--kek-type")]
    public string? KekType { get; set; }

    [CliOption("--kek-url")]
    public string? KekUrl { get; set; }

    [CliOption("--kek-vault-resource-id")]
    public string? KekVaultResourceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mobile")]
    public string? Mobile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }

    [CliOption("--postal-code")]
    public string? PostalCode { get; set; }

    [CliOption("--resource-group-for-managed-disk")]
    public string? ResourceGroupForManagedDisk { get; set; }

    [CliOption("--staging-storage-account")]
    public int? StagingStorageAccount { get; set; }

    [CliOption("--state-or-province")]
    public string? StateOrProvince { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--street-address1")]
    public string? StreetAddress1 { get; set; }

    [CliOption("--street-address2")]
    public string? StreetAddress2 { get; set; }

    [CliOption("--street-address3")]
    public string? StreetAddress3 { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--transfer-all-blobs")]
    public bool? TransferAllBlobs { get; set; }

    [CliFlag("--transfer-all-files")]
    public bool? TransferAllFiles { get; set; }

    [CliOption("--transfer-configuration-type")]
    public string? TransferConfigurationType { get; set; }

    [CliOption("--transfer-filter-details")]
    public string? TransferFilterDetails { get; set; }
}