using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "policies", "delete")]
public record GcloudIamPoliciesDeleteOptions(
[property: PositionalArgument] string PolicyId,
[property: CommandSwitch("--attachment-point")] string AttachmentPoint,
[property: CommandSwitch("--kind")] string Kind
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}