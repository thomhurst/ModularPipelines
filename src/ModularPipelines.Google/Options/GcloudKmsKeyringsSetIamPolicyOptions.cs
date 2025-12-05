using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keyrings", "set-iam-policy")]
public record GcloudKmsKeyringsSetIamPolicyOptions(
[property: CliArgument] string Keyring,
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}