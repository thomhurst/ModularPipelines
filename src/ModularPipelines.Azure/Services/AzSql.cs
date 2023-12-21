using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSql
{
    public AzSql(
        AzSqlDb db,
        AzSqlDbArc dbArc,
        AzSqlDw dw,
        AzSqlElasticPool elasticPool,
        AzSqlFailoverGroup failoverGroup,
        AzSqlInstanceFailoverGroup instanceFailoverGroup,
        AzSqlInstanceFailoverGroupArc instanceFailoverGroupArc,
        AzSqlInstancePool instancePool,
        AzSqlMi mi,
        AzSqlMiArc miArc,
        AzSqlMidb midb,
        AzSqlMidbArc midbArc,
        AzSqlRecoverableMidb recoverableMidb,
        AzSqlServer server,
        AzSqlServerArc serverArc,
        AzSqlStg stg,
        AzSqlVirtualCluster virtualCluster,
        AzSqlVm vm,
        ICommand internalCommand
    )
    {
        Db = db;
        DbArc = dbArc;
        Dw = dw;
        ElasticPool = elasticPool;
        FailoverGroup = failoverGroup;
        InstanceFailoverGroup = instanceFailoverGroup;
        InstanceFailoverGroupArc = instanceFailoverGroupArc;
        InstancePool = instancePool;
        Mi = mi;
        MiArc = miArc;
        Midb = midb;
        MidbArc = midbArc;
        RecoverableMidb = recoverableMidb;
        Server = server;
        ServerArc = serverArc;
        Stg = stg;
        VirtualCluster = virtualCluster;
        Vm = vm;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlDb Db { get; }

    public AzSqlDbArc DbArc { get; }

    public AzSqlDw Dw { get; }

    public AzSqlElasticPool ElasticPool { get; }

    public AzSqlFailoverGroup FailoverGroup { get; }

    public AzSqlInstanceFailoverGroup InstanceFailoverGroup { get; }

    public AzSqlInstanceFailoverGroupArc InstanceFailoverGroupArc { get; }

    public AzSqlInstancePool InstancePool { get; }

    public AzSqlMi Mi { get; }

    public AzSqlMiArc MiArc { get; }

    public AzSqlMidb Midb { get; }

    public AzSqlMidbArc MidbArc { get; }

    public AzSqlRecoverableMidb RecoverableMidb { get; }

    public AzSqlServer Server { get; }

    public AzSqlServerArc ServerArc { get; }

    public AzSqlStg Stg { get; }

    public AzSqlVirtualCluster VirtualCluster { get; }

    public AzSqlVm Vm { get; }

    public async Task<CommandResult> Down(AzSqlDownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDownOptions(), token);
    }

    public async Task<CommandResult> ListUsages(AzSqlListUsagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowUsage(AzSqlShowUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Up(AzSqlUpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlUpOptions(), token);
    }
}