using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql")]
public class AzSqlDb
{
    public AzSqlDb(
        AzSqlDbAdvancedThreatProtectionSetting advancedThreatProtectionSetting,
        AzSqlDbAuditPolicy auditPolicy,
        AzSqlDbClassification classification,
        AzSqlDbGeoBackup geoBackup,
        AzSqlDbLedgerDigestUploads ledgerDigestUploads,
        AzSqlDbLtrBackup ltrBackup,
        AzSqlDbLtrPolicy ltrPolicy,
        AzSqlDbOp op,
        AzSqlDbReplica replica,
        AzSqlDbStrPolicy strPolicy,
        AzSqlDbTde tde,
        ICommand internalCommand
    )
    {
        AdvancedThreatProtectionSetting = advancedThreatProtectionSetting;
        AuditPolicy = auditPolicy;
        Classification = classification;
        GeoBackup = geoBackup;
        LedgerDigestUploads = ledgerDigestUploads;
        LtrBackup = ltrBackup;
        LtrPolicy = ltrPolicy;
        Op = op;
        Replica = replica;
        StrPolicy = strPolicy;
        Tde = tde;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlDbAdvancedThreatProtectionSetting AdvancedThreatProtectionSetting { get; }

    public AzSqlDbAuditPolicy AuditPolicy { get; }

    public AzSqlDbClassification Classification { get; }

    public AzSqlDbGeoBackup GeoBackup { get; }

    public AzSqlDbLedgerDigestUploads LedgerDigestUploads { get; }

    public AzSqlDbLtrBackup LtrBackup { get; }

    public AzSqlDbLtrPolicy LtrPolicy { get; }

    public AzSqlDbOp Op { get; }

    public AzSqlDbReplica Replica { get; }

    public AzSqlDbStrPolicy StrPolicy { get; }

    public AzSqlDbTde Tde { get; }

    public async Task<CommandResult> Copy(AzSqlDbCopyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzSqlDbCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlDbDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbDeleteOptions(), token);
    }

    public async Task<CommandResult> Export(AzSqlDbExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzSqlDbImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSqlDbListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbListOptions(), token);
    }

    public async Task<CommandResult> ListDeleted(AzSqlDbListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbListDeletedOptions(), token);
    }

    public async Task<CommandResult> ListEditions(AzSqlDbListEditionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsages(AzSqlDbListUsagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbListUsagesOptions(), token);
    }

    public async Task<CommandResult> Rename(AzSqlDbRenameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzSqlDbRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSqlDbShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbShowOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzSqlDbShowConnectionStringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowDeleted(AzSqlDbShowDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSqlDbUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbUpdateOptions(), token);
    }
}