using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "verify")]
public record AwsKmsVerifyOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--message")] string Message,
[property: CommandSwitch("--signature")] string Signature,
[property: CommandSwitch("--signing-algorithm")] string SigningAlgorithm
) : AwsOptions
{
    [CommandSwitch("--message-type")]
    public string? MessageType { get; set; }

    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}