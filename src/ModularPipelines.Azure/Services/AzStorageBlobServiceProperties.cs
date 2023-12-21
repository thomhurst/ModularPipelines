using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob")]
public class AzStorageBlobServiceProperties
{
    public AzStorageBlobServiceProperties(
        AzStorageBlobServicePropertiesDeletePolicy deletePolicy,
        AzStorageBlobServicePropertiesUpdate update,
        ICommand internalCommand
    )
    {
        DeletePolicy = deletePolicy;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageBlobServicePropertiesDeletePolicy DeletePolicy { get; }

    public AzStorageBlobServicePropertiesUpdate UpdateCommands { get; }

    public async Task<CommandResult> Show(AzStorageBlobServicePropertiesShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobServicePropertiesShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStorageBlobServicePropertiesUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobServicePropertiesUpdateOptions(), token);
    }
}