using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse")]
public class AzSynapseSql
{
    public AzSynapseSql(
        AzSynapseSqlAdAdmin adAdmin,
        AzSynapseSqlAuditPolicy auditPolicy,
        AzSynapseSqlPool pool
    )
    {
        AdAdmin = adAdmin;
        AuditPolicy = auditPolicy;
        Pool = pool;
    }

    public AzSynapseSqlAdAdmin AdAdmin { get; }

    public AzSynapseSqlAuditPolicy AuditPolicy { get; }

    public AzSynapseSqlPool Pool { get; }
}