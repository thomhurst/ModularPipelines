using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "tickets", "show")]
public record AzSupportTicketsShowOptions(
[property: CliOption("--ticket-name")] string TicketName
) : AzOptions;