using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm-pca", "create-permission")]
public record AwsAcmPcaCreatePermissionOptions(
[property: CommandSwitch("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CommandSwitch("--principal")] string Principal,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--source-account")]
    public string? SourceAccount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}