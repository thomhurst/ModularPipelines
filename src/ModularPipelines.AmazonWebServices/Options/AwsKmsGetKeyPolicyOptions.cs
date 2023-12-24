using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "get-key-policy")]
public record AwsKmsGetKeyPolicyOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--policy-name")] string PolicyName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}