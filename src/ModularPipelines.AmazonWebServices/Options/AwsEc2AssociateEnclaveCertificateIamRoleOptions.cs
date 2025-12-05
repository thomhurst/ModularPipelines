using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-enclave-certificate-iam-role")]
public record AwsEc2AssociateEnclaveCertificateIamRoleOptions(
[property: CliOption("--certificate-arn")] string CertificateArn,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}