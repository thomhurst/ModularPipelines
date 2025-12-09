using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "tickets", "update")]
public record AzSupportTicketsUpdateOptions(
[property: CliOption("--ticket-name")] string TicketName
) : AzOptions
{
    [CliOption("--contact-additional-emails")]
    public string? ContactAdditionalEmails { get; set; }

    [CliOption("--contact-country")]
    public int? ContactCountry { get; set; }

    [CliOption("--contact-email")]
    public string? ContactEmail { get; set; }

    [CliOption("--contact-first-name")]
    public string? ContactFirstName { get; set; }

    [CliOption("--contact-language")]
    public string? ContactLanguage { get; set; }

    [CliOption("--contact-last-name")]
    public string? ContactLastName { get; set; }

    [CliOption("--contact-method")]
    public string? ContactMethod { get; set; }

    [CliOption("--contact-phone-number")]
    public string? ContactPhoneNumber { get; set; }

    [CliOption("--contact-timezone")]
    public string? ContactTimezone { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}