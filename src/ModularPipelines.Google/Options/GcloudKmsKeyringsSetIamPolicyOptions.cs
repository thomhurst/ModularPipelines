using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keyrings", "set-iam-policy")]
public record GcloudKmsKeyringsSetIamPolicyOptions(
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}