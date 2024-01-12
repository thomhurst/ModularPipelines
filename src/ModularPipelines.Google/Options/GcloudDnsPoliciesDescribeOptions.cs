using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "policies", "describe")]
public record GcloudDnsPoliciesDescribeOptions(
[property: PositionalArgument] string Policy
) : GcloudOptions;