using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "operations", "describe")]
public record GcloudVmwareOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;