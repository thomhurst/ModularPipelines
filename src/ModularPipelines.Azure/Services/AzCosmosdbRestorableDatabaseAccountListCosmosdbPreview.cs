using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restorable-database-account", "list")]
public class AzCosmosdbRestorableDatabaseAccountListCosmosdbPreview
{
    public AzCosmosdbRestorableDatabaseAccountListCosmosdbPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzCosmosdbRestorableDatabaseAccountListCosmosdbPreviewExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbRestorableDatabaseAccountListCosmosdbPreviewExtensionOptions(), token);
    }
}