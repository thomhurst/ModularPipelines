using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci")]
public class AzAzurestackhciVirtualharddisk
{
    public AzAzurestackhciVirtualharddisk(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAzurestackhciVirtualharddiskCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciVirtualharddiskDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualharddiskDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAzurestackhciVirtualharddiskListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualharddiskListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciVirtualharddiskShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualharddiskShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciVirtualharddiskUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualharddiskUpdateOptions(), token);
    }
}