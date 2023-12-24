using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-context-keys-for-custom-policy")]
public record AwsIamGetContextKeysForCustomPolicyOptions(
[property: CommandSwitch("--policy-input-list")] string[] PolicyInputList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}