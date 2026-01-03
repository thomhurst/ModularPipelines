using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "account")]
public class AzNetappfilesAccountBackupPolicy
{
    public AzNetappfilesAccountBackupPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetappfilesAccountBackupPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesAccountBackupPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetappfilesAccountBackupPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetappfilesAccountBackupPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetappfilesAccountBackupPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesAccountBackupPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyWaitOptions(), cancellationToken: token);
    }
}