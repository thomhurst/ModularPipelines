using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection")]
public class AzDataprotectionBackupPolicy
{
    public AzDataprotectionBackupPolicy(
        AzDataprotectionBackupPolicyRetentionRule retentionRule,
        AzDataprotectionBackupPolicyTag tag,
        AzDataprotectionBackupPolicyTrigger trigger,
        ICommand internalCommand
    )
    {
        RetentionRule = retentionRule;
        Tag = tag;
        Trigger = trigger;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDataprotectionBackupPolicyRetentionRule RetentionRule { get; }

    public AzDataprotectionBackupPolicyTag Tag { get; }

    public AzDataprotectionBackupPolicyTrigger Trigger { get; }

    public async Task<CommandResult> Create(AzDataprotectionBackupPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDataprotectionBackupPolicyDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDefaultPolicyTemplate(AzDataprotectionBackupPolicyGetDefaultPolicyTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDataprotectionBackupPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDataprotectionBackupPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDataprotectionBackupPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupPolicyUpdateOptions(), token);
    }
}