using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCosmosdb
{
    public AzCosmosdb(
        AzCosmosdbCassandra cassandra,
        AzCosmosdbCopy copy,
        AzCosmosdbCreate create,
        AzCosmosdbGremlin gremlin,
        AzCosmosdbIdentity identity,
        AzCosmosdbKeys keys,
        AzCosmosdbList list,
        AzCosmosdbLocations locations,
        AzCosmosdbMongocluster mongocluster,
        AzCosmosdbMongodb mongodb,
        AzCosmosdbNetworkRule networkRule,
        AzCosmosdbPostgres postgres,
        AzCosmosdbPrivateEndpointConnection privateEndpointConnection,
        AzCosmosdbPrivateLinkResource privateLinkResource,
        AzCosmosdbRestorableDatabaseAccount restorableDatabaseAccount,
        AzCosmosdbRestore restore,
        AzCosmosdbService service,
        AzCosmosdbShow show,
        AzCosmosdbSql sql,
        AzCosmosdbTable table,
        AzCosmosdbUpdate update,
        ICommand internalCommand
    )
    {
        Cassandra = cassandra;
        Copy = copy;
        CreateCommands = create;
        Gremlin = gremlin;
        Identity = identity;
        Keys = keys;
        ListCommands = list;
        Locations = locations;
        Mongocluster = mongocluster;
        Mongodb = mongodb;
        NetworkRule = networkRule;
        Postgres = postgres;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        RestorableDatabaseAccount = restorableDatabaseAccount;
        RestoreCommands = restore;
        Service = service;
        ShowCommands = show;
        Sql = sql;
        Table = table;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbCassandra Cassandra { get; }

    public AzCosmosdbCopy Copy { get; }

    public AzCosmosdbCreate CreateCommands { get; }

    public AzCosmosdbGremlin Gremlin { get; }

    public AzCosmosdbIdentity Identity { get; }

    public AzCosmosdbKeys Keys { get; }

    public AzCosmosdbList ListCommands { get; }

    public AzCosmosdbLocations Locations { get; }

    public AzCosmosdbMongocluster Mongocluster { get; }

    public AzCosmosdbMongodb Mongodb { get; }

    public AzCosmosdbNetworkRule NetworkRule { get; }

    public AzCosmosdbPostgres Postgres { get; }

    public AzCosmosdbPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzCosmosdbPrivateLinkResource PrivateLinkResource { get; }

    public AzCosmosdbRestorableDatabaseAccount RestorableDatabaseAccount { get; }

    public AzCosmosdbRestore RestoreCommands { get; }

    public AzCosmosdbService Service { get; }

    public AzCosmosdbShow ShowCommands { get; }

    public AzCosmosdbSql Sql { get; }

    public AzCosmosdbTable Table { get; }

    public AzCosmosdbUpdate UpdateCommands { get; }

    public async Task<CommandResult> CheckNameExists(AzCosmosdbCheckNameExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbCheckNameExistsOptions(), token);
    }

    public async Task<CommandResult> Create(AzCosmosdbCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbDeleteOptions(), token);
    }

    public async Task<CommandResult> FailoverPriorityChange(AzCosmosdbFailoverPriorityChangeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbListOptions(), token);
    }

    public async Task<CommandResult> Restore(AzCosmosdbRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzCosmosdbUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbUpdateOptions(), token);
    }
}