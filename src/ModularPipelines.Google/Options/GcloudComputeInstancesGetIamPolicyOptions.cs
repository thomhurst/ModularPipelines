using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "get-iam-policy")]
public record GcloudComputeInstancesGetIamPolicyOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Zone
) : GcloudOptions;