using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "describe")]
public record GcloudIamWorkforcePoolsDescribeOptions(
[property: CliArgument] string WorkforcePool,
[property: CliArgument] string Location
) : GcloudOptions;