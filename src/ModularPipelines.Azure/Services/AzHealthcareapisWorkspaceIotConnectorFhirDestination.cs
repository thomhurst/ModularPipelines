using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "workspace", "iot-connector")]
public class AzHealthcareapisWorkspaceIotConnectorFhirDestination
{
    public AzHealthcareapisWorkspaceIotConnectorFhirDestination(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzHealthcareapisWorkspaceIotConnectorFhirDestinationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHealthcareapisWorkspaceIotConnectorFhirDestinationDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzHealthcareapisWorkspaceIotConnectorFhirDestinationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzHealthcareapisWorkspaceIotConnectorFhirDestinationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzHealthcareapisWorkspaceIotConnectorFhirDestinationUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzHealthcareapisWorkspaceIotConnectorFhirDestinationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceIotConnectorFhirDestinationWaitOptions(), token);
    }
}