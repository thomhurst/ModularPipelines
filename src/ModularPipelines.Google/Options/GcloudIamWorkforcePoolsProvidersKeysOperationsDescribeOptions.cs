using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "keys", "operations", "describe")]
public record GcloudIamWorkforcePoolsProvidersKeysOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Key,
[property: CliArgument] string Location,
[property: CliArgument] string Provider,
[property: CliArgument] string WorkforcePool
) : GcloudOptions;