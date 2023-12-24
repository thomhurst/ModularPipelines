using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "update-certificate")]
public record AwsTransferUpdateCertificateOptions(
[property: CommandSwitch("--certificate-id")] string CertificateId
) : AwsOptions
{
    [CommandSwitch("--active-date")]
    public long? ActiveDate { get; set; }

    [CommandSwitch("--inactive-date")]
    public long? InactiveDate { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}