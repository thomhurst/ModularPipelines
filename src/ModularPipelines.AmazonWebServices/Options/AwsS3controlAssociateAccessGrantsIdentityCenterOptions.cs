using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "associate-access-grants-identity-center")]
public record AwsS3controlAssociateAccessGrantsIdentityCenterOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--identity-center-arn")] string IdentityCenterArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}