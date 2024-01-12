using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudResourceManager
{
    public GcloudResourceManager(
        GcloudResourceManagerFolders folders,
        GcloudResourceManagerOrgPolicies orgPolicies,
        GcloudResourceManagerTags tags
    )
    {
        Folders = folders;
        OrgPolicies = orgPolicies;
        Tags = tags;
    }

    public GcloudResourceManagerFolders Folders { get; }

    public GcloudResourceManagerOrgPolicies OrgPolicies { get; }

    public GcloudResourceManagerTags Tags { get; }
}