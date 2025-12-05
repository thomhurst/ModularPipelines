using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "operations", "describe")]
public record GcloudIamWorkforcePoolsProvidersOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location,
[property: CliArgument] string Provider,
[property: CliArgument] string WorkforcePool
) : GcloudOptions;