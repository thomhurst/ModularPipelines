using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-context-keys-for-principal-policy")]
public record AwsIamGetContextKeysForPrincipalPolicyOptions(
[property: CommandSwitch("--policy-source-arn")] string PolicySourceArn
) : AwsOptions
{
    [CommandSwitch("--policy-input-list")]
    public string[]? PolicyInputList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}