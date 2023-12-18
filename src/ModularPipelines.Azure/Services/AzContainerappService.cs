using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp")]
public class AzContainerappService
{
    public AzContainerappService(
        AzContainerappServiceKafka kafka,
        AzContainerappServiceMariadb mariadb,
        AzContainerappServicePostgres postgres,
        AzContainerappServiceQdrant qdrant,
        AzContainerappServiceRedis redis,
        ICommand internalCommand
    )
    {
        Kafka = kafka;
        Mariadb = mariadb;
        Postgres = postgres;
        Qdrant = qdrant;
        Redis = redis;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappServiceKafka Kafka { get; }

    public AzContainerappServiceMariadb Mariadb { get; }

    public AzContainerappServicePostgres Postgres { get; }

    public AzContainerappServiceQdrant Qdrant { get; }

    public AzContainerappServiceRedis Redis { get; }

    public async Task<CommandResult> List(AzContainerappServiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}