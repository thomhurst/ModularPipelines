using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "get-iam-policy")]
public record GcloudComputeImagesGetIamPolicyOptions(
[property: CliArgument] string Image
) : GcloudOptions;