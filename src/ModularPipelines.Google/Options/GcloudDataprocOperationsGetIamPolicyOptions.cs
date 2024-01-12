using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "operations", "get-iam-policy")]
public record GcloudDataprocOperationsGetIamPolicyOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Region
) : GcloudOptions;