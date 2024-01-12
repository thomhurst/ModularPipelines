using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "jobs", "wait")]
public record GcloudDataprocJobsWaitOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Region
) : GcloudOptions;