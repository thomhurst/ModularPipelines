using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "set-iam-policy")]
public record GcloudComputeInstancesSetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Zone,
[property: CliArgument] string PolicyFile
) : GcloudOptions;