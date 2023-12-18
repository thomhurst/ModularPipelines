using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
public class AzCosmosdbSql
{
    public AzCosmosdbSql(
        AzCosmosdbSqlContainer container,
        AzCosmosdbSqlDatabase database,
        AzCosmosdbSqlRestorableContainer restorableContainer,
        AzCosmosdbSqlRestorableDatabase restorableDatabase,
        AzCosmosdbSqlRestorableResource restorableResource,
        AzCosmosdbSqlRole role,
        AzCosmosdbSqlStoredProcedure storedProcedure,
        AzCosmosdbSqlTrigger trigger,
        AzCosmosdbSqlUserDefinedFunction userDefinedFunction,
        ICommand internalCommand
    )
    {
        Container = container;
        Database = database;
        RestorableContainer = restorableContainer;
        RestorableDatabase = restorableDatabase;
        RestorableResource = restorableResource;
        Role = role;
        StoredProcedure = storedProcedure;
        Trigger = trigger;
        UserDefinedFunction = userDefinedFunction;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbSqlContainer Container { get; }

    public AzCosmosdbSqlDatabase Database { get; }

    public AzCosmosdbSqlRestorableContainer RestorableContainer { get; }

    public AzCosmosdbSqlRestorableDatabase RestorableDatabase { get; }

    public AzCosmosdbSqlRestorableResource RestorableResource { get; }

    public AzCosmosdbSqlRole Role { get; }

    public AzCosmosdbSqlStoredProcedure StoredProcedure { get; }

    public AzCosmosdbSqlTrigger Trigger { get; }

    public AzCosmosdbSqlUserDefinedFunction UserDefinedFunction { get; }

    public async Task<CommandResult> RetrieveLatestBackupTime(AzCosmosdbSqlRetrieveLatestBackupTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}