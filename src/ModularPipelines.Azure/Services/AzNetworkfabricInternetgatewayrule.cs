using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricInternetgatewayrule
{
    public AzNetworkfabricInternetgatewayrule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricInternetgatewayruleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricInternetgatewayruleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayruleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricInternetgatewayruleListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayruleListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricInternetgatewayruleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayruleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricInternetgatewayruleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayruleUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricInternetgatewayruleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricInternetgatewayruleWaitOptions(), token);
    }
}

