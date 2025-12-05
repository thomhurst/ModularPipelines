using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "describe")]
public record GcloudIamWorkforcePoolsProvidersDescribeOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location,
[property: CliArgument] string WorkforcePool
) : GcloudOptions;