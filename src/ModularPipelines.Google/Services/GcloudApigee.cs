using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudApigee
{
    public GcloudApigee(
        GcloudApigeeApis apis,
        GcloudApigeeApplications applications,
        GcloudApigeeDeployments deployments,
        GcloudApigeeDevelopers developers,
        GcloudApigeeEnvironments environments,
        GcloudApigeeOrganizations organizations,
        GcloudApigeeProducts products
    )
    {
        Apis = apis;
        Applications = applications;
        Deployments = deployments;
        Developers = developers;
        Environments = environments;
        Organizations = organizations;
        Products = products;
    }

    public GcloudApigeeApis Apis { get; }

    public GcloudApigeeApplications Applications { get; }

    public GcloudApigeeDeployments Deployments { get; }

    public GcloudApigeeDevelopers Developers { get; }

    public GcloudApigeeEnvironments Environments { get; }

    public GcloudApigeeOrganizations Organizations { get; }

    public GcloudApigeeProducts Products { get; }
}