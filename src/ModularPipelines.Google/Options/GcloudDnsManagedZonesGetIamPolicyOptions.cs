using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "managed-zones", "get-iam-policy")]
public record GcloudDnsManagedZonesGetIamPolicyOptions(
[property: CliArgument] string Zone
) : GcloudOptions;