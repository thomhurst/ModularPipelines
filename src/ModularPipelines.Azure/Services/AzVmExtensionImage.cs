using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "extension")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageListOptions(), token);
    }

    public async Task<CommandResult> ListNames(AzVmExtensionImageListNamesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageListNamesOptions(), token);
    }

    public async Task<CommandResult> ListVersions(AzVmExtensionImageListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageListVersionsOptions(), token);
    }

    public async Task<CommandResult> Show(AzVmExtensionImageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmExtensionImageShowOptions(), token);
    }
}

