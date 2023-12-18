using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network")]
public class AzMobileNetworkSlice
{
    public AzMobileNetworkSlice(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMobileNetworkSliceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkSliceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSliceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMobileNetworkSliceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkSliceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSliceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkSliceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSliceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkSliceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSliceWaitOptions(), token);
    }
}