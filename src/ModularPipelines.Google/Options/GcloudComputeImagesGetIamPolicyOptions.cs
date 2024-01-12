using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "get-iam-policy")]
public record GcloudComputeImagesGetIamPolicyOptions(
[property: PositionalArgument] string Image
) : GcloudOptions;