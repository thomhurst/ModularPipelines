using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "put-key-policy")]
public record AwsKmsPutKeyPolicyOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}