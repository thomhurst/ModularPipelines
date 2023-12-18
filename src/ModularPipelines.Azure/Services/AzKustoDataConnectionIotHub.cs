using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "data-connection")]
public class AzKustoDataConnectionIotHub
{
    public AzKustoDataConnectionIotHub(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzKustoDataConnectionIotHubCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DataConnectionValidation(AzKustoDataConnectionIotHubDataConnectionValidationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDataConnectionIotHubDataConnectionValidationOptions(), token);
    }

    public async Task<CommandResult> Update(AzKustoDataConnectionIotHubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDataConnectionIotHubUpdateOptions(), token);
    }
}