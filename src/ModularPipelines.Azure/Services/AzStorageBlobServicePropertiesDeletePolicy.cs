using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "service-properties")]
public class AzStorageBlobServicePropertiesDeletePolicy
{
    public AzStorageBlobServicePropertiesDeletePolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzStorageBlobServicePropertiesDeletePolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobServicePropertiesDeletePolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStorageBlobServicePropertiesDeletePolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobServicePropertiesDeletePolicyUpdateOptions(), token);
    }
}