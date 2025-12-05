using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "operations", "get-iam-policy")]
public record GcloudDataprocOperationsGetIamPolicyOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;