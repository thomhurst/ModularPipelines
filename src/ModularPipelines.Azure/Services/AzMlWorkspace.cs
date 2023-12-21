using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml")]
public class AzMlWorkspace
{
    public AzMlWorkspace(
        AzMlWorkspaceOutboundRule outboundRule,
        AzMlWorkspacePrivateEndpoint privateEndpoint,
        ICommand internalCommand
    )
    {
        OutboundRule = outboundRule;
        PrivateEndpoint = privateEndpoint;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMlWorkspaceOutboundRule OutboundRule { get; }

    public AzMlWorkspacePrivateEndpoint PrivateEndpoint { get; }

    public async Task<CommandResult> Create(AzMlWorkspaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMlWorkspaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceDeleteOptions(), token);
    }

    public async Task<CommandResult> Diagnose(AzMlWorkspaceDiagnoseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceDiagnoseOptions(), token);
    }

    public async Task<CommandResult> List(AzMlWorkspaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceListOptions(), token);
    }

    public async Task<CommandResult> ListKeys(AzMlWorkspaceListKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceListKeysOptions(), token);
    }

    public async Task<CommandResult> ProvisionNetwork(AzMlWorkspaceProvisionNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Share(AzMlWorkspaceShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMlWorkspaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceShowOptions(), token);
    }

    public async Task<CommandResult> SyncKeys(AzMlWorkspaceSyncKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceSyncKeysOptions(), token);
    }

    public async Task<CommandResult> Update(AzMlWorkspaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateDependencies(AzMlWorkspaceUpdateDependenciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspaceUpdateDependenciesOptions(), token);
    }
}