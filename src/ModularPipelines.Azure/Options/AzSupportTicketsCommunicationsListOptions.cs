using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "tickets", "communications", "list")]
public record AzSupportTicketsCommunicationsListOptions(
[property: CliOption("--ticket-name")] string TicketName
) : AzOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }
}