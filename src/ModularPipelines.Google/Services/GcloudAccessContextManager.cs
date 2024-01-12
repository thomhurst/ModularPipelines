using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAccessContextManager
{
    public GcloudAccessContextManager(
        GcloudAccessContextManagerAuthorizedOrgs authorizedOrgs,
        GcloudAccessContextManagerCloudBindings cloudBindings,
        GcloudAccessContextManagerLevels levels,
        GcloudAccessContextManagerPerimeters perimeters,
        GcloudAccessContextManagerPolicies policies
    )
    {
        AuthorizedOrgs = authorizedOrgs;
        CloudBindings = cloudBindings;
        Levels = levels;
        Perimeters = perimeters;
        Policies = policies;
    }

    public GcloudAccessContextManagerAuthorizedOrgs AuthorizedOrgs { get; }

    public GcloudAccessContextManagerCloudBindings CloudBindings { get; }

    public GcloudAccessContextManagerLevels Levels { get; }

    public GcloudAccessContextManagerPerimeters Perimeters { get; }

    public GcloudAccessContextManagerPolicies Policies { get; }
}