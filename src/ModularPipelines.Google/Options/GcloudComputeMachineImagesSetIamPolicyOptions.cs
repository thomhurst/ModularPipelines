using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "machine-images", "set-iam-policy")]
public record GcloudComputeMachineImagesSetIamPolicyOptions(
[property: PositionalArgument] string MachineImage,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;