using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "operations", "describe")]
public record GcloudNetworkManagementOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;