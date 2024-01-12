using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "policies", "describe")]
public record GcloudAccessContextManagerPoliciesDescribeOptions(
[property: PositionalArgument] string Policy
) : GcloudOptions;