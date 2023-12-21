using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp")]
public class AzContainerappAddOn
{
    public AzContainerappAddOn(
        AzContainerappAddOnKafka kafka,
        AzContainerappAddOnMariadb mariadb,
        AzContainerappAddOnPostgres postgres,
        AzContainerappAddOnQdrant qdrant,
        AzContainerappAddOnRedis redis,
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

    public AzContainerappAddOnKafka Kafka { get; }

    public AzContainerappAddOnMariadb Mariadb { get; }

    public AzContainerappAddOnPostgres Postgres { get; }

    public AzContainerappAddOnQdrant Qdrant { get; }

    public AzContainerappAddOnRedis Redis { get; }

    public async Task<CommandResult> List(AzContainerappAddOnListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}