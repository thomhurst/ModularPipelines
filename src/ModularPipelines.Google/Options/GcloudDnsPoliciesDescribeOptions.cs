using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "policies", "describe")]
public record GcloudDnsPoliciesDescribeOptions(
[property: CliArgument] string Policy
) : GcloudOptions;