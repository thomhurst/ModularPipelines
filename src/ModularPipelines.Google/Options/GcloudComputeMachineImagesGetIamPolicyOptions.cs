using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "get-iam-policy")]
public record GcloudComputeMachineImagesGetIamPolicyOptions(
[property: CliArgument] string MachineImage
) : GcloudOptions;