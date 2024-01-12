using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudTelcoAutomation
{
    public GcloudTelcoAutomation(
        GcloudTelcoAutomationOperations operations,
        GcloudTelcoAutomationOrchestrationCluster orchestrationCluster
    )
    {
        Operations = operations;
        OrchestrationCluster = orchestrationCluster;
    }

    public GcloudTelcoAutomationOperations Operations { get; }

    public GcloudTelcoAutomationOrchestrationCluster OrchestrationCluster { get; }
}