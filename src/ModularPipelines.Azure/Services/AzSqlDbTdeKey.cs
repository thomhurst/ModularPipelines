using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "tde")]
public class AzSqlDbTdeKey
{
    public AzSqlDbTdeKey(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Revalidate(AzSqlDbTdeKeyRevalidateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbTdeKeyRevalidateOptions(), token);
    }

    public async Task<CommandResult> Revert(AzSqlDbTdeKeyRevertOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbTdeKeyRevertOptions(), token);
    }
}