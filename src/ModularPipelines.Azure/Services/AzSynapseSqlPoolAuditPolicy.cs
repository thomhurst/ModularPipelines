using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql", "pool")]
public class AzSynapseSqlPoolAuditPolicy
{
    public AzSynapseSqlPoolAuditPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzSynapseSqlPoolAuditPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseSqlPoolAuditPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSynapseSqlPoolAuditPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseSqlPoolAuditPolicyUpdateOptions(), token);
    }
}