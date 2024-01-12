using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudMl
{
    public GcloudMl(
        GcloudMlLanguage language,
        GcloudMlSpeech speech,
        GcloudMlVideo video,
        GcloudMlVision vision
    )
    {
        Language = language;
        Speech = speech;
        Video = video;
        Vision = vision;
    }

    public GcloudMlLanguage Language { get; }

    public GcloudMlSpeech Speech { get; }

    public GcloudMlVideo Video { get; }

    public GcloudMlVision Vision { get; }
}