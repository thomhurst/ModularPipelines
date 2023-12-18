using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql")]
public class AzSqlMidb
{
    public AzSqlMidb(
        AzSqlMidbAdvancedThreatProtectionSetting advancedThreatProtectionSetting,
        AzSqlMidbCopy copy,
        AzSqlMidbLedgerDigestUploads ledgerDigestUploads,
        AzSqlMidbLogReplay logReplay,
        AzSqlMidbLtrBackup ltrBackup,
        AzSqlMidbLtrPolicy ltrPolicy,
        AzSqlMidbMove move,
        AzSqlMidbShortTermRetentionPolicy shortTermRetentionPolicy,
        ICommand internalCommand
    )
    {
        AdvancedThreatProtectionSetting = advancedThreatProtectionSetting;
        Copy = copy;
        LedgerDigestUploads = ledgerDigestUploads;
        LogReplay = logReplay;
        LtrBackup = ltrBackup;
        LtrPolicy = ltrPolicy;
        Move = move;
        ShortTermRetentionPolicy = shortTermRetentionPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlMidbAdvancedThreatProtectionSetting AdvancedThreatProtectionSetting { get; }

    public AzSqlMidbCopy Copy { get; }

    public AzSqlMidbLedgerDigestUploads LedgerDigestUploads { get; }

    public AzSqlMidbLogReplay LogReplay { get; }

    public AzSqlMidbLtrBackup LtrBackup { get; }

    public AzSqlMidbLtrPolicy LtrPolicy { get; }

    public AzSqlMidbMove Move { get; }

    public AzSqlMidbShortTermRetentionPolicy ShortTermRetentionPolicy { get; }

    public async Task<CommandResult> Create(AzSqlMidbCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlMidbDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlMidbListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbListOptions(), token);
    }

    public async Task<CommandResult> ListDeleted(AzSqlMidbListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbListDeletedOptions(), token);
    }

    public async Task<CommandResult> Recover(AzSqlMidbRecoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzSqlMidbRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSqlMidbShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlMidbUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbUpdateOptions(), token);
    }
}