using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory")]
public class AzDatafactoryManagedPrivateEndpoint
{
    public AzDatafactoryManagedPrivateEndpoint(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDatafactoryManagedPrivateEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatafactoryManagedPrivateEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryManagedPrivateEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDatafactoryManagedPrivateEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDatafactoryManagedPrivateEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryManagedPrivateEndpointShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatafactoryManagedPrivateEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryManagedPrivateEndpointUpdateOptions(), token);
    }
}