using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "remove-iam-policy-binding")]
public record GcloudComputeMachineImagesRemoveIamPolicyBindingOptions(
[property: CliArgument] string MachineImage,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;