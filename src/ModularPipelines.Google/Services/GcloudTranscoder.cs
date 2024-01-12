using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudTranscoder
{
    public GcloudTranscoder(
        GcloudTranscoderJobs jobs,
        GcloudTranscoderTemplates templates
    )
    {
        Jobs = jobs;
        Templates = templates;
    }

    public GcloudTranscoderJobs Jobs { get; }

    public GcloudTranscoderTemplates Templates { get; }
}