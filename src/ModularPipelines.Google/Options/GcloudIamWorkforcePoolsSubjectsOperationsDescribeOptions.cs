using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "subjects", "operations", "describe")]
public record GcloudIamWorkforcePoolsSubjectsOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location,
[property: CliArgument] string Subject,
[property: CliArgument] string WorkforcePool
) : GcloudOptions;