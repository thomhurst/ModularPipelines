using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb")]
public class AzCosmosdbMongodb
{
    public AzCosmosdbMongodb(
        AzCosmosdbMongodbCollection collection,
        AzCosmosdbMongodbDatabase database,
        AzCosmosdbMongodbRestorableCollection restorableCollection,
        AzCosmosdbMongodbRestorableDatabase restorableDatabase,
        AzCosmosdbMongodbRestorableResource restorableResource,
        AzCosmosdbMongodbRole role,
        AzCosmosdbMongodbUser user,
        ICommand internalCommand
    )
    {
        Collection = collection;
        Database = database;
        RestorableCollection = restorableCollection;
        RestorableDatabase = restorableDatabase;
        RestorableResource = restorableResource;
        Role = role;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbMongodbCollection Collection { get; }

    public AzCosmosdbMongodbDatabase Database { get; }

    public AzCosmosdbMongodbRestorableCollection RestorableCollection { get; }

    public AzCosmosdbMongodbRestorableDatabase RestorableDatabase { get; }

    public AzCosmosdbMongodbRestorableResource RestorableResource { get; }

    public AzCosmosdbMongodbRole Role { get; }

    public AzCosmosdbMongodbUser User { get; }

    public async Task<CommandResult> RetrieveLatestBackupTime(AzCosmosdbMongodbRetrieveLatestBackupTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}