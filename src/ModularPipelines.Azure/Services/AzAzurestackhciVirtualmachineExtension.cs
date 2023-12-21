using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "virtualmachine")]
public class AzAzurestackhciVirtualmachineExtension
{
    public AzAzurestackhciVirtualmachineExtension(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAzurestackhciVirtualmachineExtensionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciVirtualmachineExtensionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineExtensionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAzurestackhciVirtualmachineExtensionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciVirtualmachineExtensionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineExtensionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciVirtualmachineExtensionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzAzurestackhciVirtualmachineExtensionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciVirtualmachineExtensionWaitOptions(), token);
    }
}