using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "providers", "operations", "describe")]
public record GcloudIamWorkforcePoolsProvidersOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string WorkforcePool
) : GcloudOptions;