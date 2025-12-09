using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-context-keys-for-principal-policy")]
public record AwsIamGetContextKeysForPrincipalPolicyOptions(
[property: CliOption("--policy-source-arn")] string PolicySourceArn
) : AwsOptions
{
    [CliOption("--policy-input-list")]
    public string[]? PolicyInputList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}