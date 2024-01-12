using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "machine-images", "get-iam-policy")]
public record GcloudComputeMachineImagesGetIamPolicyOptions(
[property: PositionalArgument] string MachineImage
) : GcloudOptions;