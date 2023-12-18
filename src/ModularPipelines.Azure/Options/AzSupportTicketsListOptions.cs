using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "list")]
public record AzSupportTicketsListOptions(
[property: CommandSwitch("--ticket-name")] string TicketName
) : AzOptions
{
    [CommandSwitch("--filters")]
    public string? Filters { get; set; }
}