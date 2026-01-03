using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("urestackhci")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAzurestackhciGalleryimageDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAzurestackhciGalleryimageListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAzurestackhciGalleryimageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAzurestackhciGalleryimageUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAzurestackhciGalleryimageUpdateOptions(), cancellationToken: token);
    }
}