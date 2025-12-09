using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("config")]
public class AzConfigParamPersist
{
    public AzConfigParamPersist(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzConfigParamPersistDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfigParamPersistDeleteOptions(), token);
    }

    public async Task<CommandResult> Off(AzConfigParamPersistOffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfigParamPersistOffOptions(), token);
    }

    public async Task<CommandResult> On(AzConfigParamPersistOnOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfigParamPersistOnOptions(), token);
    }

    public async Task<CommandResult> Show(AzConfigParamPersistShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfigParamPersistShowOptions(), token);
    }
}