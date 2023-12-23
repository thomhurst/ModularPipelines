using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "describe-certificate")]
public record AwsDsDescribeCertificateOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}