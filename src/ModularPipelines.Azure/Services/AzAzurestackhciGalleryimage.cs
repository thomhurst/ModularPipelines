using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci")]
public class AzAzurestackhciGalleryimage
{
    public AzAzurestackhciGalleryimage(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAzurestackhciGalleryimageCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciGalleryimageDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAzurestackhciGalleryimageListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciGalleryimageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciGalleryimageUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageUpdateOptions(), token);
    }
}