using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support")]
public class AzSupportTickets
{
    public AzSupportTickets(
        AzSupportTicketsCommunications communications,
        ICommand internalCommand
    )
    {
        Communications = communications;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSupportTicketsCommunications Communications { get; }

    public async Task<CommandResult> Create(AzSupportTicketsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSupportTicketsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSupportTicketsShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSupportTicketsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}