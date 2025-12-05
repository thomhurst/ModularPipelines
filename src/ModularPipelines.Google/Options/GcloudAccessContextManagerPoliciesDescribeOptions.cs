using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "policies", "describe")]
public record GcloudAccessContextManagerPoliciesDescribeOptions(
[property: CliArgument] string Policy
) : GcloudOptions;