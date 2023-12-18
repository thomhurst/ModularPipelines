using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi")]
public class AzSqlMiServerConfigurationOption
{
    public AzSqlMiServerConfigurationOption(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzSqlMiServerConfigurationOptionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzSqlMiServerConfigurationOptionSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiServerConfigurationOptionSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlMiServerConfigurationOptionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiServerConfigurationOptionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSqlMiServerConfigurationOptionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiServerConfigurationOptionWaitOptions(), token);
    }
}