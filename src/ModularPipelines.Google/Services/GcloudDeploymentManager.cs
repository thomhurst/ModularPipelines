using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDeploymentManager
{
    public GcloudDeploymentManager(
        GcloudDeploymentManagerDeployments deployments,
        GcloudDeploymentManagerManifests manifests,
        GcloudDeploymentManagerOperations operations,
        GcloudDeploymentManagerResources resources,
        GcloudDeploymentManagerTypes types
    )
    {
        Deployments = deployments;
        Manifests = manifests;
        Operations = operations;
        Resources = resources;
        Types = types;
    }

    public GcloudDeploymentManagerDeployments Deployments { get; }

    public GcloudDeploymentManagerManifests Manifests { get; }

    public GcloudDeploymentManagerOperations Operations { get; }

    public GcloudDeploymentManagerResources Resources { get; }

    public GcloudDeploymentManagerTypes Types { get; }
}