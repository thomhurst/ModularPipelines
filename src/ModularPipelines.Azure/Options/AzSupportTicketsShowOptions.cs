using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "show")]
public record AzSupportTicketsShowOptions(
[property: CommandSwitch("--ticket-name")] string TicketName
) : AzOptions
{
}