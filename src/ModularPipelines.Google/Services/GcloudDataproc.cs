using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDataproc
{
    public GcloudDataproc(
        GcloudDataprocAutoscalingPolicies autoscalingPolicies,
        GcloudDataprocBatches batches,
        GcloudDataprocClusters clusters,
        GcloudDataprocJobs jobs,
        GcloudDataprocNodeGroups nodeGroups,
        GcloudDataprocOperations operations,
        GcloudDataprocWorkflowTemplates workflowTemplates
    )
    {
        AutoscalingPolicies = autoscalingPolicies;
        Batches = batches;
        Clusters = clusters;
        Jobs = jobs;
        NodeGroups = nodeGroups;
        Operations = operations;
        WorkflowTemplates = workflowTemplates;
    }

    public GcloudDataprocAutoscalingPolicies AutoscalingPolicies { get; }

    public GcloudDataprocBatches Batches { get; }

    public GcloudDataprocClusters Clusters { get; }

    public GcloudDataprocJobs Jobs { get; }

    public GcloudDataprocNodeGroups NodeGroups { get; }

    public GcloudDataprocOperations Operations { get; }

    public GcloudDataprocWorkflowTemplates WorkflowTemplates { get; }
}