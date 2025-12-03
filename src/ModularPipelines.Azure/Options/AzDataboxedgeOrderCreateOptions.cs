using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databoxedge", "order", "create")]
public record AzDataboxedgeOrderCreateOptions(
[property: CliOption("--address-line1")] string AddressLine1,
[property: CliOption("--city")] string City,
[property: CliOption("--company-name")] string CompanyName,
[property: CliOption("--contact-person")] string ContactPerson,
[property: CliOption("--country")] int Country,
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--email-list")] string EmailList,
[property: CliOption("--phone")] string Phone,
[property: CliOption("--postal-code")] string PostalCode,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--state")] string State,
[property: CliOption("--status")] string Status
) : AzOptions
{
    [CliOption("--address-line2")]
    public string? AddressLine2 { get; set; }

    [CliOption("--address-line3")]
    public string? AddressLine3 { get; set; }

    [CliOption("--comments")]
    public string? Comments { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}