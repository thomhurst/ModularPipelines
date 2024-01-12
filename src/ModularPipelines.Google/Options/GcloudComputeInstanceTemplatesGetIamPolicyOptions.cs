using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instance-templates", "get-iam-policy")]
public record GcloudComputeInstanceTemplatesGetIamPolicyOptions(
[property: PositionalArgument] string InstanceTemplate
) : GcloudOptions;