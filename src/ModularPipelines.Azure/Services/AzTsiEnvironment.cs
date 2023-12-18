using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi")]
public class AzTsiEnvironment
{
    public AzTsiEnvironment(
        AzTsiEnvironmentGen1 gen1,
        AzTsiEnvironmentGen2 gen2,
        ICommand internalCommand
    )
    {
        Gen1 = gen1;
        Gen2 = gen2;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzTsiEnvironmentGen1 Gen1 { get; }

    public AzTsiEnvironmentGen2 Gen2 { get; }

    public async Task<CommandResult> Delete(AzTsiEnvironmentDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzTsiEnvironmentDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzTsiEnvironmentListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzTsiEnvironmentListOptions(), token);
    }

    public async Task<CommandResult> Show(AzTsiEnvironmentShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzTsiEnvironmentShowOptions(), token);
    }
}