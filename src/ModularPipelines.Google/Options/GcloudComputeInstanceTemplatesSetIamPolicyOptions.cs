using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-templates", "set-iam-policy")]
public record GcloudComputeInstanceTemplatesSetIamPolicyOptions(
[property: PositionalArgument] string InstanceTemplate,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;