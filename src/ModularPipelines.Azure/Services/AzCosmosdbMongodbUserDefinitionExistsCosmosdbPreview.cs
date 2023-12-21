using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "user", "definition", "exists")]
public class AzCosmosdbMongodbUserDefinitionExistsCosmosdbPreview
{
    public AzCosmosdbMongodbUserDefinitionExistsCosmosdbPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzCosmosdbMongodbUserDefinitionExistsCosmosdbPreviewExtensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}