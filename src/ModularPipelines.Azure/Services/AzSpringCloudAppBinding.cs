using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app")]
public class AzSpringCloudAppBinding
{
    public AzSpringCloudAppBinding(
        AzSpringCloudAppBindingCosmos cosmos,
        AzSpringCloudAppBindingMysql mysql,
        AzSpringCloudAppBindingRedis redis,
        ICommand internalCommand
    )
    {
        Cosmos = cosmos;
        Mysql = mysql;
        Redis = redis;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudAppBindingCosmos Cosmos { get; }

    public AzSpringCloudAppBindingMysql Mysql { get; }

    public AzSpringCloudAppBindingRedis Redis { get; }

    public async Task<CommandResult> List(AzSpringCloudAppBindingListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzSpringCloudAppBindingRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudAppBindingShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}