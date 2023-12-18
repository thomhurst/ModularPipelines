using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

