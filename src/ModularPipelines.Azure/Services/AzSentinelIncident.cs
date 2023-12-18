using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel")]
public class AzSentinelIncident
{
    public AzSentinelIncident(
        AzSentinelIncidentComment comment,
        AzSentinelIncidentRelation relation,
        ICommand internalCommand
    )
    {
        Comment = comment;
        Relation = relation;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSentinelIncidentComment Comment { get; }

    public AzSentinelIncidentRelation Relation { get; }

    public async Task<CommandResult> Create(AzSentinelIncidentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTeam(AzSentinelIncidentCreateTeamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSentinelIncidentDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSentinelIncidentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAlert(AzSentinelIncidentListAlertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBookmark(AzSentinelIncidentListBookmarkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEntity(AzSentinelIncidentListEntityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunPlaybook(AzSentinelIncidentRunPlaybookOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSentinelIncidentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelIncidentShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSentinelIncidentUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelIncidentUpdateOptions(), token);
    }
}

