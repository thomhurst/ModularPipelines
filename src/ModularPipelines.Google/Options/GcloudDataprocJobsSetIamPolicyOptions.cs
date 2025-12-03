using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "set-iam-policy")]
public record GcloudDataprocJobsSetIamPolicyOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;