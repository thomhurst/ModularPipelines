using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databricks")]
public class AzDatabricksAccessConnector
{
    public AzDatabricksAccessConnector(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDatabricksAccessConnectorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatabricksAccessConnectorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksAccessConnectorDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDatabricksAccessConnectorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksAccessConnectorListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDatabricksAccessConnectorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksAccessConnectorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatabricksAccessConnectorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksAccessConnectorUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatabricksAccessConnectorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksAccessConnectorWaitOptions(), token);
    }
}