using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("immersive-stream")]
public class GcloudImmersiveStreamXr
{
    public GcloudImmersiveStreamXr(
        GcloudImmersiveStreamXrContents contents,
        GcloudImmersiveStreamXrInstances instances,
        GcloudImmersiveStreamXrOperations operations
    )
    {
        Contents = contents;
        Instances = instances;
        Operations = operations;
    }

    public GcloudImmersiveStreamXrContents Contents { get; }

    public GcloudImmersiveStreamXrInstances Instances { get; }

    public GcloudImmersiveStreamXrOperations Operations { get; }
}