using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "restorable-database-account", "show")]
public class AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreview
{
    public AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreviewExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreviewExtensionOptions(), token);
    }
}