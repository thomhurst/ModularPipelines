using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "policies", "delete")]
public record GcloudAccessContextManagerPoliciesDeleteOptions(
[property: PositionalArgument] string Policy
) : GcloudOptions;