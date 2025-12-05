using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge", "order", "update")]
public record AzDataboxedgeOrderUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-line1")]
    public string? AddressLine1 { get; set; }

    [CliOption("--address-line2")]
    public string? AddressLine2 { get; set; }

    [CliOption("--address-line3")]
    public string? AddressLine3 { get; set; }

    [CliOption("--city")]
    public string? City { get; set; }

    [CliOption("--comments")]
    public string? Comments { get; set; }

    [CliOption("--company-name")]
    public string? CompanyName { get; set; }

    [CliOption("--contact-person")]
    public string? ContactPerson { get; set; }

    [CliOption("--country")]
    public int? Country { get; set; }

    [CliOption("--device-name")]
    public string? DeviceName { get; set; }

    [CliOption("--email-list")]
    public string? EmailList { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }

    [CliOption("--postal-code")]
    public string? PostalCode { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}