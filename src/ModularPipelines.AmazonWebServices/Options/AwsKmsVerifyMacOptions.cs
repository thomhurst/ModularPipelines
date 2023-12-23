using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "verify-mac")]
public record AwsKmsVerifyMacOptions(
[property: CommandSwitch("--message")] string Message,
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--mac-algorithm")] string MacAlgorithm,
[property: CommandSwitch("--mac")] string Mac
) : AwsOptions
{
    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}