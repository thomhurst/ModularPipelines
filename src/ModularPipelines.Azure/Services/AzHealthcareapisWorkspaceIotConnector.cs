using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "workspace")]
public class AzHealthcareapisWorkspaceIotConnector
{
    public AzHealthcareapisWorkspaceIotConnector(
        AzHealthcareapisWorkspaceIotConnectorFhirDestination fhirDestination,
        ICommand internalCommand
    )
    {
        FhirDestination = fhirDestination;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzHealthcareapisWorkspaceIotConnectorFhirDestination FhirDestination { get; }

    public async Task<CommandResult> Create(AzHealthcareapisWorkspaceIotConnectorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHealthcareapisWorkspaceIotConnectorDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzHealthcareapisWorkspaceIotConnectorListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzHealthcareapisWorkspaceIotConnectorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceIotConnectorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzHealthcareapisWorkspaceIotConnectorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceIotConnectorUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzHealthcareapisWorkspaceIotConnectorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceIotConnectorWaitOptions(), token);
    }
}