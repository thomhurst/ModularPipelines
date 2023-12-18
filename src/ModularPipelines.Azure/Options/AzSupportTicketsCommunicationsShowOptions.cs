using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "communications", "show")]
public record AzSupportTicketsCommunicationsShowOptions(
[property: CommandSwitch("--communication-name")] string CommunicationName,
[property: CommandSwitch("--ticket-name")] string TicketName
) : AzOptions;