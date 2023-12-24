using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "delete-permission")]
public record AwsAcmPcaDeletePermissionOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CommandSwitch("--principal")] string Principal
) : AwsOptions
{
    [CommandSwitch("--source-account")]
    public string? SourceAccount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}