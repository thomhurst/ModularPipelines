using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "extension")]
public class AzVmssExtensionImage
{
    public AzVmssExtensionImage(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzVmssExtensionImageListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssExtensionImageListOptions(), token);
    }

    public async Task<CommandResult> ListNames(AzVmssExtensionImageListNamesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssExtensionImageListNamesOptions(), token);
    }

    public async Task<CommandResult> ListVersions(AzVmssExtensionImageListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssExtensionImageListVersionsOptions(), token);
    }

    public async Task<CommandResult> Show(AzVmssExtensionImageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmssExtensionImageShowOptions(), token);
    }
}