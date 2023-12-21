using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "data-connection")]
public class AzKustoDataConnectionEventHub
{
    public AzKustoDataConnectionEventHub(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKustoDataConnectionEventHubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DataConnectionValidation(AzKustoDataConnectionEventHubDataConnectionValidationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDataConnectionEventHubDataConnectionValidationOptions(), token);
    }

    public async Task<CommandResult> Update(AzKustoDataConnectionEventHubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDataConnectionEventHubUpdateOptions(), token);
    }
}