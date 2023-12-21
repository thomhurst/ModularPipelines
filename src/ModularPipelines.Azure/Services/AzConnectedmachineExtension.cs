using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine")]
public class AzConnectedmachineExtension
{
    public AzConnectedmachineExtension(
        AzConnectedmachineExtensionImage image,
        ICommand internalCommand
    )
    {
        Image = image;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzConnectedmachineExtensionImage Image { get; }

    public async Task<CommandResult> Create(AzConnectedmachineExtensionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConnectedmachineExtensionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineExtensionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConnectedmachineExtensionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzConnectedmachineExtensionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineExtensionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzConnectedmachineExtensionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineExtensionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzConnectedmachineExtensionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineExtensionWaitOptions(), token);
    }
}