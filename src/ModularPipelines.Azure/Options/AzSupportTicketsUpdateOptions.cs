using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "update")]
public record AzSupportTicketsUpdateOptions(
[property: CommandSwitch("--ticket-name")] string TicketName
) : AzOptions
{
    [CommandSwitch("--contact-additional-emails")]
    public string? ContactAdditionalEmails { get; set; }

    [CommandSwitch("--contact-country")]
    public int? ContactCountry { get; set; }

    [CommandSwitch("--contact-email")]
    public string? ContactEmail { get; set; }

    [CommandSwitch("--contact-first-name")]
    public string? ContactFirstName { get; set; }

    [CommandSwitch("--contact-language")]
    public string? ContactLanguage { get; set; }

    [CommandSwitch("--contact-last-name")]
    public string? ContactLastName { get; set; }

    [CommandSwitch("--contact-method")]
    public string? ContactMethod { get; set; }

    [CommandSwitch("--contact-phone-number")]
    public string? ContactPhoneNumber { get; set; }

    [CommandSwitch("--contact-timezone")]
    public string? ContactTimezone { get; set; }

    [CommandSwitch("--severity")]
    public string? Severity { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}