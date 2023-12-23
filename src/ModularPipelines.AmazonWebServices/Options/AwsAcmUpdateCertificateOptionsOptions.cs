using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm", "update-certificate-options")]
public record AwsAcmUpdateCertificateOptionsOptions(
[property: CommandSwitch("--certificate-arn")] string CertificateArn,
[property: CommandSwitch("--options")] string Options
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}