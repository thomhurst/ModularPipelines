using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db")]
public class AzSqlDbTde
{
    public AzSqlDbTde(
        AzSqlDbTdeKey key,
        ICommand internalCommand
    )
    {
        Key = key;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlDbTdeKey Key { get; }

    public async Task<CommandResult> Set(AzSqlDbTdeSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSqlDbTdeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbTdeShowOptions(), token);
    }
}