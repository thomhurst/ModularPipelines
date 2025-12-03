using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "describe")]
public record GcloudDataprocJobsDescribeOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Region
) : GcloudOptions;