using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts")]
public class GcloudArtifactsDocker
{
    public GcloudArtifactsDocker(
        GcloudArtifactsDockerImages images,
        GcloudArtifactsDockerTags tags
    )
    {
        Images = images;
        Tags = tags;
    }

    public GcloudArtifactsDockerImages Images { get; }

    public GcloudArtifactsDockerTags Tags { get; }
}