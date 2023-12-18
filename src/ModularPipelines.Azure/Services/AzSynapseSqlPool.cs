using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql")]
public class AzSynapseSqlPool
{
    public AzSynapseSqlPool(
        AzSynapseSqlPoolAuditPolicy auditPolicy,
        AzSynapseSqlPoolClassification classification,
        AzSynapseSqlPoolTde tde,
        AzSynapseSqlPoolThreatPolicy threatPolicy,
        ICommand internalCommand
    )
    {
        AuditPolicy = auditPolicy;
        Classification = classification;
        Tde = tde;
        ThreatPolicy = threatPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSynapseSqlPoolAuditPolicy AuditPolicy { get; }

    public AzSynapseSqlPoolClassification Classification { get; }

    public AzSynapseSqlPoolTde Tde { get; }

    public AzSynapseSqlPoolThreatPolicy ThreatPolicy { get; }

    public async Task<CommandResult> Create(AzSynapseSqlPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSynapseSqlPoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseSqlPoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSynapseSqlPoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeleted(AzSynapseSqlPoolListDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Pause(AzSynapseSqlPoolPauseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseSqlPoolPauseOptions(), token);
    }

    public async Task<CommandResult> Restore(AzSynapseSqlPoolRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resume(AzSynapseSqlPoolResumeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseSqlPoolResumeOptions(), token);
    }

    public async Task<CommandResult> Show(AzSynapseSqlPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseSqlPoolShowOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzSynapseSqlPoolShowConnectionStringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSynapseSqlPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseSqlPoolUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSynapseSqlPoolWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}