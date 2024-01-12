using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "jobs", "delete")]
public record GcloudDataprocJobsDeleteOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Region
) : GcloudOptions;