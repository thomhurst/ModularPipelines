using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account")]
public class AzStorageAccountBlobServiceProperties
{
    public AzStorageAccountBlobServiceProperties(
        AzStorageAccountBlobServicePropertiesCorsRule corsRule,
        ICommand internalCommand
    )
    {
        CorsRule = corsRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageAccountBlobServicePropertiesCorsRule CorsRule { get; }

    public async Task<CommandResult> Show(AzStorageAccountBlobServicePropertiesShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageAccountBlobServicePropertiesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}