using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudInfraManager
{
    public GcloudInfraManager(
        GcloudInfraManagerDeployments deployments,
        GcloudInfraManagerPreviews previews,
        GcloudInfraManagerResources resources,
        GcloudInfraManagerRevisions revisions
    )
    {
        Deployments = deployments;
        Previews = previews;
        Resources = resources;
        Revisions = revisions;
    }

    public GcloudInfraManagerDeployments Deployments { get; }

    public GcloudInfraManagerPreviews Previews { get; }

    public GcloudInfraManagerResources Resources { get; }

    public GcloudInfraManagerRevisions Revisions { get; }
}