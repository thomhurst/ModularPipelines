using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-signing-certificate")]
public record AwsIamUpdateSigningCertificateOptions(
[property: CommandSwitch("--certificate-id")] string CertificateId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}