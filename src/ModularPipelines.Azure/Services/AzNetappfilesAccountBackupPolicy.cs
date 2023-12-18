using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesAccountBackupPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetappfilesAccountBackupPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetappfilesAccountBackupPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetappfilesAccountBackupPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesAccountBackupPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupPolicyWaitOptions(), token);
    }
}