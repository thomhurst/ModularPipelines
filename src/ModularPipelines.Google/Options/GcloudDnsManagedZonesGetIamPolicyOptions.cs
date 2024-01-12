using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "managed-zones", "get-iam-policy")]
public record GcloudDnsManagedZonesGetIamPolicyOptions(
[property: PositionalArgument] string Zone
) : GcloudOptions;