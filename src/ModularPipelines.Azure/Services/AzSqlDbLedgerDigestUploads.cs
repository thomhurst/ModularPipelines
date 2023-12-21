using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db")]
public class AzSqlDbLedgerDigestUploads
{
    public AzSqlDbLedgerDigestUploads(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Disable(AzSqlDbLedgerDigestUploadsDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbLedgerDigestUploadsDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzSqlDbLedgerDigestUploadsEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSqlDbLedgerDigestUploadsShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbLedgerDigestUploadsShowOptions(), token);
    }
}