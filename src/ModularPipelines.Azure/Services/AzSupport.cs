using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzSupport
{
    public AzSupport(
        AzSupportServices services,
        AzSupportTickets tickets
    )
    {
        Services = services;
        Tickets = tickets;
    }

    public AzSupportServices Services { get; }

    public AzSupportTickets Tickets { get; }
}