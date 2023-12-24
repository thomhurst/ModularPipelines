using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "generate-mac")]
public record AwsKmsGenerateMacOptions(
[property: CommandSwitch("--message")] string Message,
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--mac-algorithm")] string MacAlgorithm
) : AwsOptions
{
    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}