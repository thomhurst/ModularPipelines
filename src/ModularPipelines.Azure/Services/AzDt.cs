using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzDt
{
    public AzDt(
        AzDtDataHistory dataHistory,
        AzDtEndpoint endpoint,
        AzDtIdentity identity,
        AzDtJob job,
        AzDtModel model,
        AzDtNetwork network,
        AzDtRoleAssignment roleAssignment,
        AzDtRoute route,
        AzDtTwin twin,
        ICommand internalCommand
    )
    {
        DataHistory = dataHistory;
        Endpoint = endpoint;
        Identity = identity;
        Job = job;
        Model = model;
        Network = network;
        RoleAssignment = roleAssignment;
        Route = route;
        Twin = twin;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDtDataHistory DataHistory { get; }

    public AzDtEndpoint Endpoint { get; }

    public AzDtIdentity Identity { get; }

    public AzDtJob Job { get; }

    public AzDtModel Model { get; }

    public AzDtNetwork Network { get; }

    public AzDtRoleAssignment RoleAssignment { get; }

    public AzDtRoute Route { get; }

    public AzDtTwin Twin { get; }

    public async Task<CommandResult> Create(AzDtCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDtDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDtListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reset(AzDtResetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDtShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzDtWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}