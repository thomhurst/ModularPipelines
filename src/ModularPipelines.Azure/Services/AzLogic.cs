using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzLogic
{
    public AzLogic(
        AzLogicIntegrationAccount integrationAccount,
        AzLogicWorkflow workflow
    )
    {
        IntegrationAccount = integrationAccount;
        Workflow = workflow;
    }

    public AzLogicIntegrationAccount IntegrationAccount { get; }

    public AzLogicWorkflow Workflow { get; }
}