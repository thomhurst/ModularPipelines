using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "jobs", "list")]
public record GcloudMlEngineJobsListOptions : GcloudOptions;