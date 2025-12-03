using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-context-keys-for-custom-policy")]
public record AwsIamGetContextKeysForCustomPolicyOptions(
[property: CliOption("--policy-input-list")] string[] PolicyInputList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}