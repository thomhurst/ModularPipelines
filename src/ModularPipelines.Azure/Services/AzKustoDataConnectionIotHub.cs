using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "data-connection")]
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