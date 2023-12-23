using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "delete-certificate-authority")]
public record AwsAcmPcaDeleteCertificateAuthorityOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn
) : AwsOptions
{
    [CommandSwitch("--permanent-deletion-time-in-days")]
    public int? PermanentDeletionTimeInDays { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}