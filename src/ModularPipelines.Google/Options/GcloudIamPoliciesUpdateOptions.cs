using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "policies", "update")]
public record GcloudIamPoliciesUpdateOptions(
[property: CliArgument] string PolicyId,
[property: CliOption("--attachment-point")] string AttachmentPoint,
[property: CliOption("--kind")] string Kind,
[property: CliOption("--policy-file")] string PolicyFile
) : GcloudOptions;