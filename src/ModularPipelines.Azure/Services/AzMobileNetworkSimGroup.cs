using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "sim")]
public class AzMobileNetworkSimGroup
{
    public AzMobileNetworkSimGroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BulkDeleteSims(AzMobileNetworkSimGroupBulkDeleteSimsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BulkUploadSims(AzMobileNetworkSimGroupBulkUploadSimsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzMobileNetworkSimGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkSimGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMobileNetworkSimGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkSimGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkSimGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkSimGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupWaitOptions(), token);
    }
}