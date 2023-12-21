using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql")]
public class AzSqlElasticPool
{
    public AzSqlElasticPool(
        AzSqlElasticPoolOp op,
        ICommand internalCommand
    )
    {
        Op = op;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlElasticPoolOp Op { get; }

    public async Task<CommandResult> Create(AzSqlElasticPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlElasticPoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlElasticPoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlElasticPoolListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlElasticPoolListOptions(), token);
    }

    public async Task<CommandResult> ListDbs(AzSqlElasticPoolListDbsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlElasticPoolListDbsOptions(), token);
    }

    public async Task<CommandResult> ListEditions(AzSqlElasticPoolListEditionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSqlElasticPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlElasticPoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlElasticPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlElasticPoolUpdateOptions(), token);
    }
}