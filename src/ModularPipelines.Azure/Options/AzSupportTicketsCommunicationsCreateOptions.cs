using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "tickets", "communications", "create")]
public record AzSupportTicketsCommunicationsCreateOptions(
[property: CliOption("--communication-body")] string CommunicationBody,
[property: CliOption("--communication-name")] string CommunicationName,
[property: CliOption("--communication-subject")] string CommunicationSubject,
[property: CliOption("--ticket-name")] string TicketName
) : AzOptions
{
    [CliOption("--communication-sender")]
    public string? CommunicationSender { get; set; }
}