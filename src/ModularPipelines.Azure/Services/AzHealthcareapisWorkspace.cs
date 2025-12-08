using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcareapis")]
public class AzHealthcareapisWorkspace
{
    public AzHealthcareapisWorkspace(
        AzHealthcareapisWorkspaceDicomService dicomService,
        AzHealthcareapisWorkspaceFhirService fhirService,
        AzHealthcareapisWorkspaceIotConnector iotConnector,
        AzHealthcareapisWorkspacePrivateEndpointConnection privateEndpointConnection,
        AzHealthcareapisWorkspacePrivateLinkResource privateLinkResource,
        ICommand internalCommand
    )
    {
        DicomService = dicomService;
        FhirService = fhirService;
        IotConnector = iotConnector;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzHealthcareapisWorkspaceDicomService DicomService { get; }

    public AzHealthcareapisWorkspaceFhirService FhirService { get; }

    public AzHealthcareapisWorkspaceIotConnector IotConnector { get; }

    public AzHealthcareapisWorkspacePrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzHealthcareapisWorkspacePrivateLinkResource PrivateLinkResource { get; }

    public async Task<CommandResult> Create(AzHealthcareapisWorkspaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHealthcareapisWorkspaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzHealthcareapisWorkspaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzHealthcareapisWorkspaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzHealthcareapisWorkspaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzHealthcareapisWorkspaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceWaitOptions(), token);
    }
}