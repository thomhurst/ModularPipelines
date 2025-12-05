using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "revoke-grant")]
public record AwsKmsRevokeGrantOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--grant-id")] string GrantId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}