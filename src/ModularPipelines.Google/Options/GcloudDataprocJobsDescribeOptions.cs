using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "jobs", "describe")]
public record GcloudDataprocJobsDescribeOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Region
) : GcloudOptions;