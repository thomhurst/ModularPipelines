using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "operations", "describe")]
public record GcloudIamWorkforcePoolsOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location,
[property: CliArgument] string WorkforcePool
) : GcloudOptions;