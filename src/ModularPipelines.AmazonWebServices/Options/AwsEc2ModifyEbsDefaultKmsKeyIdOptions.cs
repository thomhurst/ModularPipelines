using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-ebs-default-kms-key-id")]
public record AwsEc2ModifyEbsDefaultKmsKeyIdOptions(
[property: CliOption("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}