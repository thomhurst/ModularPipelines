using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "get-iam-policy")]
public record GcloudDataprocJobsGetIamPolicyOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Region
) : GcloudOptions;