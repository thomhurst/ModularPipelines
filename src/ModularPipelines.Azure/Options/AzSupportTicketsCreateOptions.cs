using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "tickets", "create")]
public record AzSupportTicketsCreateOptions(
[property: CliOption("--contact-country")] int ContactCountry,
[property: CliOption("--contact-email")] string ContactEmail,
[property: CliOption("--contact-first-name")] string ContactFirstName,
[property: CliOption("--contact-language")] string ContactLanguage,
[property: CliOption("--contact-last-name")] string ContactLastName,
[property: CliOption("--contact-method")] string ContactMethod,
[property: CliOption("--contact-timezone")] string ContactTimezone,
[property: CliOption("--description")] string Description,
[property: CliOption("--problem-classification")] string ProblemClassification,
[property: CliOption("--severity")] string Severity,
[property: CliOption("--ticket-name")] string TicketName,
[property: CliOption("--title")] string Title
) : AzOptions
{
    [CliOption("--contact-additional-emails")]
    public string? ContactAdditionalEmails { get; set; }

    [CliOption("--contact-phone-number")]
    public string? ContactPhoneNumber { get; set; }

    [CliOption("--partner-tenant-id")]
    public string? PartnerTenantId { get; set; }

    [CliOption("--quota-change-payload")]
    public string? QuotaChangePayload { get; set; }

    [CliOption("--quota-change-regions")]
    public string? QuotaChangeRegions { get; set; }

    [CliOption("--quota-change-subtype")]
    public string? QuotaChangeSubtype { get; set; }

    [CliOption("--quota-change-version")]
    public string? QuotaChangeVersion { get; set; }

    [CliFlag("--require-24-by-7-response")]
    public bool? Require24By7Response { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--technical-resource")]
    public string? TechnicalResource { get; set; }
}