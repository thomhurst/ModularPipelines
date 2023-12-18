using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databoxedge", "order", "create")]
public record AzDataboxedgeOrderCreateOptions(
[property: CommandSwitch("--address-line1")] string AddressLine1,
[property: CommandSwitch("--city")] string City,
[property: CommandSwitch("--company-name")] string CompanyName,
[property: CommandSwitch("--contact-person")] string ContactPerson,
[property: CommandSwitch("--country")] int Country,
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--email-list")] string EmailList,
[property: CommandSwitch("--phone")] string Phone,
[property: CommandSwitch("--postal-code")] string PostalCode,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--state")] string State,
[property: CommandSwitch("--status")] string Status
) : AzOptions
{
    [CommandSwitch("--address-line2")]
    public string? AddressLine2 { get; set; }

    [CommandSwitch("--address-line3")]
    public string? AddressLine3 { get; set; }

    [CommandSwitch("--comments")]
    public string? Comments { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

