using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
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
        Create = create;
        Delete = delete;
        List = list;
        Show = show;
        Start = start;
        Stop = stop;
        Update = update;
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKustoClusterCreate Create { get; }

    public AzKustoClusterDelete Delete { get; }

    public AzKustoClusterList List { get; }

    public AzKustoClusterShow Show { get; }

    public AzKustoClusterStart Start { get; }

    public AzKustoClusterStop Stop { get; }

    public AzKustoClusterUpdate Update { get; }

    public AzKustoClusterWait Wait { get; }

    public async Task<CommandResult> AddLanguageExtension(AzKustoClusterAddLanguageExtensionOptions options, CancellationToken token = default)
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
}