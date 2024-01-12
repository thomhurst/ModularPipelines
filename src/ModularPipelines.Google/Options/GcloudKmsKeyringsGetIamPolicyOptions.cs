using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keyrings", "get-iam-policy")]
public record GcloudKmsKeyringsGetIamPolicyOptions(
[property: PositionalArgument] string Keyring
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}