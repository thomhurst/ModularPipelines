using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto")]
public class AzKustoCluster
{
    public AzKustoCluster(
        AzKustoClusterCreate create,
        AzKustoClusterDelete delete,
        AzKustoClusterList list,
        AzKustoClusterShow show,
        AzKustoClusterStart start,
        AzKustoClusterStop stop,
        AzKustoClusterUpdate update,
        AzKustoClusterWait wait,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        StartCommands = start;
        StopCommands = stop;
        UpdateCommands = update;
        WaitCommands = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKustoClusterCreate CreateCommands { get; }

    public AzKustoClusterDelete DeleteCommands { get; }

    public AzKustoClusterList ListCommands { get; }

    public AzKustoClusterShow ShowCommands { get; }

    public AzKustoClusterStart StartCommands { get; }

    public AzKustoClusterStop StopCommands { get; }

    public AzKustoClusterUpdate UpdateCommands { get; }

    public AzKustoClusterWait WaitCommands { get; }

    public async Task<CommandResult> AddLanguageExtension(AzKustoClusterAddLanguageExtensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzKustoClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKustoClusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachFollowerDatabase(AzKustoClusterDetachFollowerDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DiagnoseVirtualNetwork(AzKustoClusterDiagnoseVirtualNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKustoClusterListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFollowerDatabase(AzKustoClusterListFollowerDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLanguageExtension(AzKustoClusterListLanguageExtensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOutboundNetworkDependencyEndpoint(AzKustoClusterListOutboundNetworkDependencyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSku(AzKustoClusterListSkuOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterListSkuOptions(), token);
    }

    public async Task<CommandResult> RemoveLanguageExtension(AzKustoClusterRemoveLanguageExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterRemoveLanguageExtensionOptions(), token);
    }

    public async Task<CommandResult> Show(AzKustoClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzKustoClusterStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzKustoClusterStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzKustoClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzKustoClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoClusterWaitOptions(), token);
    }
}