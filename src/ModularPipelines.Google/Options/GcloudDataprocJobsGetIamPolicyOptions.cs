using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "jobs", "get-iam-policy")]
public record GcloudDataprocJobsGetIamPolicyOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Region
) : GcloudOptions;