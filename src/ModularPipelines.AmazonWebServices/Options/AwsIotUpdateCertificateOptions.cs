using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-certificate")]
public record AwsIotUpdateCertificateOptions(
[property: CommandSwitch("--certificate-id")] string CertificateId,
[property: CommandSwitch("--new-status")] string NewStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}