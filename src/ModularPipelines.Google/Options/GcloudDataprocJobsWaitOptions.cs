using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "wait")]
public record GcloudDataprocJobsWaitOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Region
) : GcloudOptions;