using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "communications", "list")]
public record AzSupportTicketsCommunicationsListOptions(
[property: CommandSwitch("--ticket-name")] string TicketName
) : AzOptions
{
    [CommandSwitch("--filters")]
    public string? Filters { get; set; }
}