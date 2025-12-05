using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "managed-zones", "set-iam-policy")]
public record GcloudDnsManagedZonesSetIamPolicyOptions(
[property: CliArgument] string Zone,
[property: CliOption("--policy-file")] string PolicyFile
) : GcloudOptions;