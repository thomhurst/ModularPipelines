using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("healthcareapis", "workspace")]
public class AzHealthcareapisWorkspacePrivateEndpointConnection
{
    public AzHealthcareapisWorkspacePrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzHealthcareapisWorkspacePrivateEndpointConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzHealthcareapisWorkspacePrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspacePrivateEndpointConnectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzHealthcareapisWorkspacePrivateEndpointConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzHealthcareapisWorkspacePrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspacePrivateEndpointConnectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzHealthcareapisWorkspacePrivateEndpointConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspacePrivateEndpointConnectionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzHealthcareapisWorkspacePrivateEndpointConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspacePrivateEndpointConnectionWaitOptions(), cancellationToken: token);
    }
}