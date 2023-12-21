using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAmlfs
{
    public AzAmlfs(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Archive(AzAmlfsArchiveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsArchiveOptions(), token);
    }

    public async Task<CommandResult> CancelArchive(AzAmlfsCancelArchiveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsCancelArchiveOptions(), token);
    }

    public async Task<CommandResult> CheckAmlfsSubnet(AzAmlfsCheckAmlfsSubnetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsCheckAmlfsSubnetOptions(), token);
    }

    public async Task<CommandResult> Create(AzAmlfsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAmlfsDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsDeleteOptions(), token);
    }

    public async Task<CommandResult> GetSubnetsSize(AzAmlfsGetSubnetsSizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsGetSubnetsSizeOptions(), token);
    }

    public async Task<CommandResult> List(AzAmlfsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAmlfsShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAmlfsUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzAmlfsWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmlfsWaitOptions(), token);
    }
}