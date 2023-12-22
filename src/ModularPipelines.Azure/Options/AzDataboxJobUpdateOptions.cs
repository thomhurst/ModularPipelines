using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databox", "job", "update")]
public record AzDataboxJobUpdateOptions : AzOptions
{
    [CommandSwitch("--city")]
    public string? City { get; set; }

    [CommandSwitch("--company-name")]
    public string? CompanyName { get; set; }

    [CommandSwitch("--contact-name")]
    public string? ContactName { get; set; }

    [CommandSwitch("--country")]
    public int? Country { get; set; }

    [CommandSwitch("--email-list")]
    public string? EmailList { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--kek-identity")]
    public string? KekIdentity { get; set; }

    [CommandSwitch("--kek-type")]
    public string? KekType { get; set; }

    [CommandSwitch("--kek-url")]
    public string? KekUrl { get; set; }

    [CommandSwitch("--kek-vault-resource-id")]
    public string? KekVaultResourceId { get; set; }

    [CommandSwitch("--mobile")]
    public string? Mobile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--phone")]
    public string? Phone { get; set; }

    [CommandSwitch("--postal-code")]
    public string? PostalCode { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--state-or-province")]
    public string? StateOrProvince { get; set; }

    [CommandSwitch("--street-address1")]
    public string? StreetAddress1 { get; set; }

    [CommandSwitch("--street-address2")]
    public string? StreetAddress2 { get; set; }

    [CommandSwitch("--street-address3")]
    public string? StreetAddress3 { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}