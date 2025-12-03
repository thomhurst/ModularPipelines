using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "delete")]
public record GcloudDataprocJobsDeleteOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Region
) : GcloudOptions;