using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "communications", "show")]
public record AzSupportTicketsCommunicationsShowOptions(
[property: CommandSwitch("--communication-name")] string CommunicationName,
[property: CommandSwitch("--ticket-name")] string TicketName
) : AzOptions
{
}

