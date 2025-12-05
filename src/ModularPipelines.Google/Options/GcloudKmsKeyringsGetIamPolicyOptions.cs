using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keyrings", "get-iam-policy")]
public record GcloudKmsKeyringsGetIamPolicyOptions(
[property: CliArgument] string Keyring
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}