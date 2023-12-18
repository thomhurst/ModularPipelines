using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres")]
public class AzCosmosdbPostgresConfiguration
{
    public AzCosmosdbPostgresConfiguration(
        AzCosmosdbPostgresConfigurationCoordinator coordinator,
        AzCosmosdbPostgresConfigurationNode node,
        AzCosmosdbPostgresConfigurationServer server,
        ICommand internalCommand
    )
    {
        Coordinator = coordinator;
        Node = node;
        Server = server;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbPostgresConfigurationCoordinator Coordinator { get; }

    public AzCosmosdbPostgresConfigurationNode Node { get; }

    public AzCosmosdbPostgresConfigurationServer Server { get; }

    public async Task<CommandResult> List(AzCosmosdbPostgresConfigurationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbPostgresConfigurationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationShowOptions(), token);
    }
}