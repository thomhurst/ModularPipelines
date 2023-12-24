using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "update-alias")]
public record AwsKmsUpdateAliasOptions(
[property: CommandSwitch("--alias-name")] string AliasName,
[property: CommandSwitch("--target-key-id")] string TargetKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}