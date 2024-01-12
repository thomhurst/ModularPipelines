using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "managed-zones", "set-iam-policy")]
public record GcloudDnsManagedZonesSetIamPolicyOptions(
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--policy-file")] string PolicyFile
) : GcloudOptions;