using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "jobs", "list")]
public record GcloudAiPlatformJobsListOptions : GcloudOptions;