using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "kusto")]
public class AzSynapseKustoPool
{
    public AzSynapseKustoPool(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddLanguageExtension(AzSynapseKustoPoolAddLanguageExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolAddLanguageExtensionOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzSynapseKustoPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzSynapseKustoPoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> DetachFollowerDatabase(AzSynapseKustoPoolDetachFollowerDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> List(AzSynapseKustoPoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListFollowerDatabase(AzSynapseKustoPoolListFollowerDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListLanguageExtension(AzSynapseKustoPoolListLanguageExtensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListSku(AzSynapseKustoPoolListSkuOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RemoveLanguageExtension(AzSynapseKustoPoolRemoveLanguageExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolRemoveLanguageExtensionOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSynapseKustoPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzSynapseKustoPoolStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzSynapseKustoPoolStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzSynapseKustoPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzSynapseKustoPoolWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseKustoPoolWaitOptions(), cancellationToken: token);
    }
}