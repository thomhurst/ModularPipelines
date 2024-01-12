using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "policies", "get")]
public record GcloudIamPoliciesGetOptions(
[property: PositionalArgument] string PolicyId,
[property: CommandSwitch("--attachment-point")] string AttachmentPoint,
[property: CommandSwitch("--kind")] string Kind
) : GcloudOptions;