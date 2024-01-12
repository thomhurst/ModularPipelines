using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "subjects", "operations", "describe")]
public record GcloudIamWorkforcePoolsSubjectsOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Subject,
[property: PositionalArgument] string WorkforcePool
) : GcloudOptions;