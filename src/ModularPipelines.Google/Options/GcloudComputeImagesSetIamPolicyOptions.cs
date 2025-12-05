using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "set-iam-policy")]
public record GcloudComputeImagesSetIamPolicyOptions(
[property: CliArgument] string Image,
[property: CliArgument] string PolicyFile
) : GcloudOptions;