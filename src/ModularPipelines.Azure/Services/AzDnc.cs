using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDnc
{
    public AzDnc(
        AzDncController controller,
        AzDncDelegatedSubnetService delegatedSubnetService,
        AzDncOrchestratorInstanceService orchestratorInstanceService
    )
    {
        Controller = controller;
        DelegatedSubnetService = delegatedSubnetService;
        OrchestratorInstanceService = orchestratorInstanceService;
    }

    public AzDncController Controller { get; }

    public AzDncDelegatedSubnetService DelegatedSubnetService { get; }

    public AzDncOrchestratorInstanceService OrchestratorInstanceService { get; }
}