using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "sim")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> BulkUploadSims(AzMobileNetworkSimGroupBulkUploadSimsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzMobileNetworkSimGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkSimGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMobileNetworkSimGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkSimGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkSimGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkSimGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimGroupWaitOptions(), cancellationToken: token);
    }
}