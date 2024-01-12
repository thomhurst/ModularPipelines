using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "set-iam-policy")]
public record GcloudComputeInstancesSetIamPolicyOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;