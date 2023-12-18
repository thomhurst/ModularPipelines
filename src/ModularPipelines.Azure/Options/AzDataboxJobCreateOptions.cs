using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databox", "job", "create")]
public record AzDataboxJobCreateOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku,
[property: CommandSwitch("--transfer-type")] string TransferType
) : AzOptions
{
    [CommandSwitch("--city")]
    public string? City { get; set; }

    [CommandSwitch("--company-name")]
    public string? CompanyName { get; set; }

    [CommandSwitch("--contact-name")]
    public string? ContactName { get; set; }

    [CommandSwitch("--country")]
    public int? Country { get; set; }

    [CommandSwitch("--data-box-customer-disk")]
    public string? DataBoxCustomerDisk { get; set; }

    [CommandSwitch("--email-list")]
    public string? EmailList { get; set; }

    [CommandSwitch("--expected-data-size")]
    public string? ExpectedDataSize { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--kek-identity")]
    public string? KekIdentity { get; set; }

    [CommandSwitch("--kek-type")]
    public string? KekType { get; set; }

    [CommandSwitch("--kek-url")]
    public string? KekUrl { get; set; }

    [CommandSwitch("--kek-vault-resource-id")]
    public string? KekVaultResourceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mobile")]
    public string? Mobile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--phone")]
    public string? Phone { get; set; }

    [CommandSwitch("--postal-code")]
    public string? PostalCode { get; set; }

    [CommandSwitch("--resource-group-for-managed-disk")]
    public string? ResourceGroupForManagedDisk { get; set; }

    [CommandSwitch("--staging-storage-account")]
    public int? StagingStorageAccount { get; set; }

    [CommandSwitch("--state-or-province")]
    public string? StateOrProvince { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--street-address1")]
    public string? StreetAddress1 { get; set; }

    [CommandSwitch("--street-address2")]
    public string? StreetAddress2 { get; set; }

    [CommandSwitch("--street-address3")]
    public string? StreetAddress3 { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--transfer-all-blobs")]
    public bool? TransferAllBlobs { get; set; }

    [BooleanCommandSwitch("--transfer-all-files")]
    public bool? TransferAllFiles { get; set; }

    [CommandSwitch("--transfer-configuration-type")]
    public string? TransferConfigurationType { get; set; }

    [CommandSwitch("--transfer-filter-details")]
    public string? TransferFilterDetails { get; set; }
}