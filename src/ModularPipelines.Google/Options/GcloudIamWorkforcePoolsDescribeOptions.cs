using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "describe")]
public record GcloudIamWorkforcePoolsDescribeOptions(
[property: PositionalArgument] string WorkforcePool,
[property: PositionalArgument] string Location
) : GcloudOptions;