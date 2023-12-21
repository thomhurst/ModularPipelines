using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConfig
{
    public AzConfig(
        AzConfigParamPersist paramPersist,
        ICommand internalCommand
    )
    {
        ParamPersist = paramPersist;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzConfigParamPersist ParamPersist { get; }

    public async Task<CommandResult> Get(AzConfigGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfigGetOptions(), token);
    }

    public async Task<CommandResult> Set(AzConfigSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfigSetOptions(), token);
    }

    public async Task<CommandResult> Unset(AzConfigUnsetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfigUnsetOptions(), token);
    }
}