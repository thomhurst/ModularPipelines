using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "get-key-policy")]
public record AwsKmsGetKeyPolicyOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}