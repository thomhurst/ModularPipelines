using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "policies", "delete")]
public record GcloudIamPoliciesDeleteOptions(
[property: CliArgument] string PolicyId,
[property: CliOption("--attachment-point")] string AttachmentPoint,
[property: CliOption("--kind")] string Kind
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}