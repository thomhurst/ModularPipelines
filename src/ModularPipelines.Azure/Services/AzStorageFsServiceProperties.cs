using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "fs")]
public class AzStorageFsServiceProperties
{
    public AzStorageFsServiceProperties(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzStorageFsServicePropertiesShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageFsServicePropertiesShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStorageFsServicePropertiesUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageFsServicePropertiesUpdateOptions(), token);
    }
}