using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "tickets", "communications", "show")]
public record AzSupportTicketsCommunicationsShowOptions(
[property: CliOption("--communication-name")] string CommunicationName,
[property: CliOption("--ticket-name")] string TicketName
) : AzOptions;