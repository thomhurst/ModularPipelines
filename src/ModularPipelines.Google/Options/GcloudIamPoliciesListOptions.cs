using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "policies", "list")]
public record GcloudIamPoliciesListOptions(
[property: CliOption("--attachment-point")] string AttachmentPoint,
[property: CliOption("--kind")] string Kind
) : GcloudOptions
{
    [CliOption("--page_token")]
    public string? PageToken { get; set; }
}