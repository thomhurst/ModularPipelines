using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network")]
public class AzMobileNetworkAttachedDataNetwork
{
    public AzMobileNetworkAttachedDataNetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMobileNetworkAttachedDataNetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkAttachedDataNetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkAttachedDataNetworkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMobileNetworkAttachedDataNetworkListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkAttachedDataNetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkAttachedDataNetworkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkAttachedDataNetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkAttachedDataNetworkUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkAttachedDataNetworkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkAttachedDataNetworkWaitOptions(), token);
    }
}