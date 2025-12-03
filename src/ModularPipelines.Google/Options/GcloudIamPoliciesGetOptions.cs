using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "policies", "get")]
public record GcloudIamPoliciesGetOptions(
[property: CliArgument] string PolicyId,
[property: CliOption("--attachment-point")] string AttachmentPoint,
[property: CliOption("--kind")] string Kind
) : GcloudOptions;