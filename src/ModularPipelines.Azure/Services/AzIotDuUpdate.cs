using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du")]
public class AzIotDuUpdate
{
    public AzIotDuUpdate(
        AzIotDuUpdateFile file,
        AzIotDuUpdateInit init,
        ICommand internalCommand
    )
    {
        File = file;
        Init = init;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotDuUpdateFile File { get; }

    public AzIotDuUpdateInit Init { get; }

    public async Task<CommandResult> CalculateHash(AzIotDuUpdateCalculateHashOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotDuUpdateDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzIotDuUpdateImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotDuUpdateListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzIotDuUpdateShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stage(AzIotDuUpdateStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}