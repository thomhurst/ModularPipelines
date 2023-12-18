using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

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