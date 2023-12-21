using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm")]
public class AzVmExtension
{
    public AzVmExtension(
        AzVmExtensionImage image,
        ICommand internalCommand
    )
    {
        Image = image;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmExtensionImage Image { get; }

    public async Task<CommandResult> Delete(AzVmExtensionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzVmExtensionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionListOptions(), token);
    }

    public async Task<CommandResult> Set(AzVmExtensionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmExtensionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmExtensionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionWaitOptions(), token);
    }
}