using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server")]
public class AzSqlServerMsSupport
{
    public AzSqlServerMsSupport(
        AzSqlServerMsSupportAuditPolicy auditPolicy
    )
    {
        AuditPolicy = auditPolicy;
    }

    public AzSqlServerMsSupportAuditPolicy AuditPolicy { get; }
}