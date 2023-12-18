using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "order", "update")]
public record AzDataboxedgeOrderUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--address-line1")]
    public string? AddressLine1 { get; set; }

    [CommandSwitch("--address-line2")]
    public string? AddressLine2 { get; set; }

    [CommandSwitch("--address-line3")]
    public string? AddressLine3 { get; set; }

    [CommandSwitch("--city")]
    public string? City { get; set; }

    [CommandSwitch("--comments")]
    public string? Comments { get; set; }

    [CommandSwitch("--company-name")]
    public string? CompanyName { get; set; }

    [CommandSwitch("--contact-person")]
    public string? ContactPerson { get; set; }

    [CommandSwitch("--country")]
    public int? Country { get; set; }

    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--email-list")]
    public string? EmailList { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--phone")]
    public string? Phone { get; set; }

    [CommandSwitch("--postal-code")]
    public string? PostalCode { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}