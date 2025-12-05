using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "update-group-certificate-configuration")]
public record AwsGreengrassUpdateGroupCertificateConfigurationOptions(
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--certificate-expiry-in-milliseconds")]
    public string? CertificateExpiryInMilliseconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}