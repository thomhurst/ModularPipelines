using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "extension")]
public class AzVmExtensionImage
{
    public AzVmExtensionImage(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzVmExtensionImageListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListNames(AzVmExtensionImageListNamesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageListNamesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListVersions(AzVmExtensionImageListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageListVersionsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzVmExtensionImageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageShowOptions(), cancellationToken: token);
    }
}