using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "create")]
public record AzSupportTicketsCreateOptions(
[property: CommandSwitch("--contact-country")] int ContactCountry,
[property: CommandSwitch("--contact-email")] string ContactEmail,
[property: CommandSwitch("--contact-first-name")] string ContactFirstName,
[property: CommandSwitch("--contact-language")] string ContactLanguage,
[property: CommandSwitch("--contact-last-name")] string ContactLastName,
[property: CommandSwitch("--contact-method")] string ContactMethod,
[property: CommandSwitch("--contact-timezone")] string ContactTimezone,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--problem-classification")] string ProblemClassification,
[property: CommandSwitch("--severity")] string Severity,
[property: CommandSwitch("--ticket-name")] string TicketName,
[property: CommandSwitch("--title")] string Title
) : AzOptions
{
    [CommandSwitch("--contact-additional-emails")]
    public string? ContactAdditionalEmails { get; set; }

    [CommandSwitch("--contact-phone-number")]
    public string? ContactPhoneNumber { get; set; }

    [CommandSwitch("--partner-tenant-id")]
    public string? PartnerTenantId { get; set; }

    [CommandSwitch("--quota-change-payload")]
    public string? QuotaChangePayload { get; set; }

    [CommandSwitch("--quota-change-regions")]
    public string? QuotaChangeRegions { get; set; }

    [CommandSwitch("--quota-change-subtype")]
    public string? QuotaChangeSubtype { get; set; }

    [CommandSwitch("--quota-change-version")]
    public string? QuotaChangeVersion { get; set; }

    [BooleanCommandSwitch("--require-24-by-7-response")]
    public bool? Require24By7Response { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--technical-resource")]
    public string? TechnicalResource { get; set; }
}