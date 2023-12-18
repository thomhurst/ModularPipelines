using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databricks", "workspace")]
public class AzDatabricksWorkspaceVnetPeering
{
    public AzDatabricksWorkspaceVnetPeering(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDatabricksWorkspaceVnetPeeringCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatabricksWorkspaceVnetPeeringDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDatabricksWorkspaceVnetPeeringListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDatabricksWorkspaceVnetPeeringShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceVnetPeeringShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatabricksWorkspaceVnetPeeringUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceVnetPeeringUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatabricksWorkspaceVnetPeeringWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceVnetPeeringWaitOptions(), token);
    }
}