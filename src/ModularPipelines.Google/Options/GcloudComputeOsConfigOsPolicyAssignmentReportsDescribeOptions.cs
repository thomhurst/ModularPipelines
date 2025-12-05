using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "os-policy-assignment-reports", "describe")]
public record GcloudComputeOsConfigOsPolicyAssignmentReportsDescribeOptions(
[property: CliArgument] string InstanceOsPolicyAssignment,
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;