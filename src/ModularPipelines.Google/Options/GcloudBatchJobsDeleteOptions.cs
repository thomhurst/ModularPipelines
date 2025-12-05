using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "jobs", "delete")]
public record GcloudBatchJobsDeleteOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions;