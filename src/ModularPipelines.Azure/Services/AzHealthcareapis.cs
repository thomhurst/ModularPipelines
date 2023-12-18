using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzHealthcareapis
{
    public AzHealthcareapis(
        AzHealthcareapisAcr acr,
        AzHealthcareapisOperationResult operationResult,
        AzHealthcareapisPrivateEndpointConnection privateEndpointConnection,
        AzHealthcareapisPrivateLinkResource privateLinkResource,
        AzHealthcareapisService service,
        AzHealthcareapisWorkspace workspace
    )
    {
        Acr = acr;
        OperationResult = operationResult;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        Service = service;
        Workspace = workspace;
    }

    public AzHealthcareapisAcr Acr { get; }

    public AzHealthcareapisOperationResult OperationResult { get; }

    public AzHealthcareapisPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzHealthcareapisPrivateLinkResource PrivateLinkResource { get; }

    public AzHealthcareapisService Service { get; }

    public AzHealthcareapisWorkspace Workspace { get; }
}

