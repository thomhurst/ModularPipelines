using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudSql
{
    public GcloudSql(
        GcloudSqlBackups backups,
        GcloudSqlDatabases databases,
        GcloudSqlExport export,
        GcloudSqlFlags flags,
        GcloudSqlImport import,
        GcloudSqlInstances instances,
        GcloudSqlOperations operations,
        GcloudSqlSsl ssl,
        GcloudSqlTiers tiers,
        GcloudSqlUsers users,
        ICommand internalCommand
    )
    {
        Backups = backups;
        Databases = databases;
        Export = export;
        Flags = flags;
        Import = import;
        Instances = instances;
        Operations = operations;
        Ssl = ssl;
        Tiers = tiers;
        Users = users;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudSqlBackups Backups { get; }

    public GcloudSqlDatabases Databases { get; }

    public GcloudSqlExport Export { get; }

    public GcloudSqlFlags Flags { get; }

    public GcloudSqlImport Import { get; }

    public GcloudSqlInstances Instances { get; }

    public GcloudSqlOperations Operations { get; }

    public GcloudSqlSsl Ssl { get; }

    public GcloudSqlTiers Tiers { get; }

    public GcloudSqlUsers Users { get; }

    public async Task<CommandResult> Connect(GcloudSqlConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateLoginToken(GcloudSqlGenerateLoginTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudSqlGenerateLoginTokenOptions(), token);
    }

    public async Task<CommandResult> RescheduleMaintenance(GcloudSqlRescheduleMaintenanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}