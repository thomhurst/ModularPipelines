using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "policies", "delete")]
public record GcloudDnsPoliciesDeleteOptions(
[property: PositionalArgument] string Policy
) : GcloudOptions;