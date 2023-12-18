using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app")]
public class AzSpringAppBinding
{
    public AzSpringAppBinding(
        AzSpringAppBindingCosmos cosmos,
        AzSpringAppBindingMysql mysql,
        AzSpringAppBindingRedis redis,
        ICommand internalCommand
    )
    {
        Cosmos = cosmos;
        Mysql = mysql;
        Redis = redis;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringAppBindingCosmos Cosmos { get; }

    public AzSpringAppBindingMysql Mysql { get; }

    public AzSpringAppBindingRedis Redis { get; }

    public async Task<CommandResult> List(AzSpringAppBindingListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzSpringAppBindingRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringAppBindingShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}