using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "operations", "set-iam-policy")]
public record GcloudDataprocOperationsSetIamPolicyOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;