using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricFabric
{
    public AzNetworkfabricFabric(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CommitConfiguration(AzNetworkfabricFabricCommitConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkfabricFabricCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricFabricDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricFabricDeleteOptions(), token);
    }

    public async Task<CommandResult> Deprovision(AzNetworkfabricFabricDeprovisionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricFabricDeprovisionOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricFabricListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricFabricListOptions(), token);
    }

    public async Task<CommandResult> Provision(AzNetworkfabricFabricProvisionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricFabricProvisionOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricFabricShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricFabricShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricFabricUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricFabricUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricFabricWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricFabricWaitOptions(), token);
    }
}