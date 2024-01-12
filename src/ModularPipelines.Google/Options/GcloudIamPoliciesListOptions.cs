using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "policies", "list")]
public record GcloudIamPoliciesListOptions(
[property: CommandSwitch("--attachment-point")] string AttachmentPoint,
[property: CommandSwitch("--kind")] string Kind
) : GcloudOptions
{
    [CommandSwitch("--page_token")]
    public string? PageToken { get; set; }
}