using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudIdentity
{
    public GcloudIdentity(
        GcloudIdentityGroups groups
    )
    {
        Groups = groups;
    }

    public GcloudIdentityGroups Groups { get; }
}