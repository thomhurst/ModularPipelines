using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databox", "job", "update")]
public record AzDataboxJobUpdateOptions : AzOptions
{
    [CliOption("--city")]
    public string? City { get; set; }

    [CliOption("--company-name")]
    public string? CompanyName { get; set; }

    [CliOption("--contact-name")]
    public string? ContactName { get; set; }

    [CliOption("--country")]
    public int? Country { get; set; }

    [CliOption("--email-list")]
    public string? EmailList { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--kek-identity")]
    public string? KekIdentity { get; set; }

    [CliOption("--kek-type")]
    public string? KekType { get; set; }

    [CliOption("--kek-url")]
    public string? KekUrl { get; set; }

    [CliOption("--kek-vault-resource-id")]
    public string? KekVaultResourceId { get; set; }

    [CliOption("--mobile")]
    public string? Mobile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }

    [CliOption("--postal-code")]
    public string? PostalCode { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--state-or-province")]
    public string? StateOrProvince { get; set; }

    [CliOption("--street-address1")]
    public string? StreetAddress1 { get; set; }

    [CliOption("--street-address2")]
    public string? StreetAddress2 { get; set; }

    [CliOption("--street-address3")]
    public string? StreetAddress3 { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}