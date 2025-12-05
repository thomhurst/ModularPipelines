using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "vm")]
public class AzSqlVmGroup
{
    public AzSqlVmGroup(
        AzSqlVmGroupAgListener agListener,
        ICommand internalCommand
    )
    {
        AgListener = agListener;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlVmGroupAgListener AgListener { get; }

    public async Task<CommandResult> Create(AzSqlVmGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlVmGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmGroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlVmGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmGroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlVmGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlVmGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlVmGroupUpdateOptions(), token);
    }
}