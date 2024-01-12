using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "set-iam-policy")]
public record GcloudComputeImagesSetIamPolicyOptions(
[property: PositionalArgument] string Image,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;