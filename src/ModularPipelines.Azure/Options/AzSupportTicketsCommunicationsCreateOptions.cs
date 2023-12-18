using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "communications", "create")]
public record AzSupportTicketsCommunicationsCreateOptions(
[property: CommandSwitch("--communication-body")] string CommunicationBody,
[property: CommandSwitch("--communication-name")] string CommunicationName,
[property: CommandSwitch("--communication-subject")] string CommunicationSubject,
[property: CommandSwitch("--ticket-name")] string TicketName
) : AzOptions
{
    [CommandSwitch("--communication-sender")]
    public string? CommunicationSender { get; set; }
}