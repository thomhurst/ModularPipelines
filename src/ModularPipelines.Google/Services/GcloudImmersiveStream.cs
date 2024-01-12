using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudImmersiveStream
{
    public GcloudImmersiveStream(
        GcloudImmersiveStreamXr xr
    )
    {
        Xr = xr;
    }

    public GcloudImmersiveStreamXr Xr { get; }
}