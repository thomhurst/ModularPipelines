using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudSource
{
    public GcloudSource(
        GcloudSourceProjectConfigs projectConfigs,
        GcloudSourceRepos repos
    )
    {
        ProjectConfigs = projectConfigs;
        Repos = repos;
    }

    public GcloudSourceProjectConfigs ProjectConfigs { get; }

    public GcloudSourceRepos Repos { get; }
}