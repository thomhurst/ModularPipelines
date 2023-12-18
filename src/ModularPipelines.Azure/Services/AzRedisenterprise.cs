using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzRedisenterprise
{
    public AzRedisenterprise(
        AzRedisenterpriseDatabase database,
        AzRedisenterpriseOperationStatus operationStatus,
        ICommand internalCommand
    )
    {
        Database = database;
        OperationStatus = operationStatus;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzRedisenterpriseDatabase Database { get; }

    public AzRedisenterpriseOperationStatus OperationStatus { get; }

    public async Task<CommandResult> Create(AzRedisenterpriseCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzRedisenterpriseDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzRedisenterpriseListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseListOptions(), token);
    }

    public async Task<CommandResult> Show(AzRedisenterpriseShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzRedisenterpriseUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzRedisenterpriseWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisenterpriseWaitOptions(), token);
    }
}