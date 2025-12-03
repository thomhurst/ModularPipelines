using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("redisenterprise")]
public class AzRedisenterpriseDatabase
{
    public AzRedisenterpriseDatabase(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzRedisenterpriseDatabaseCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzRedisenterpriseDatabaseDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseDatabaseDeleteOptions(), token);
    }

    public async Task<CommandResult> Export(AzRedisenterpriseDatabaseExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Flush(AzRedisenterpriseDatabaseFlushOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseDatabaseFlushOptions(), token);
    }

    public async Task<CommandResult> ForceUnlink(AzRedisenterpriseDatabaseForceUnlinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzRedisenterpriseDatabaseImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzRedisenterpriseDatabaseListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListKeys(AzRedisenterpriseDatabaseListKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseDatabaseListKeysOptions(), token);
    }

    public async Task<CommandResult> RegenerateKey(AzRedisenterpriseDatabaseRegenerateKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzRedisenterpriseDatabaseShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseDatabaseShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzRedisenterpriseDatabaseUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseDatabaseUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzRedisenterpriseDatabaseWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseDatabaseWaitOptions(), token);
    }
}