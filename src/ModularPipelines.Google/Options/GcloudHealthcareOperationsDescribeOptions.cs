using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "operations", "describe")]
public record GcloudHealthcareOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;