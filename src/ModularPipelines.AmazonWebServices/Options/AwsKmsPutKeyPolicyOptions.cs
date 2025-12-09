using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "put-key-policy")]
public record AwsKmsPutKeyPolicyOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}