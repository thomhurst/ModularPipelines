using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm", "resend-validation-email")]
public record AwsAcmResendValidationEmailOptions(
[property: CommandSwitch("--certificate-arn")] string CertificateArn,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--validation-domain")] string ValidationDomain
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}