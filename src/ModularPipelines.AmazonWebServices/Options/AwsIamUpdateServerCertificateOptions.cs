using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-server-certificate")]
public record AwsIamUpdateServerCertificateOptions(
[property: CliOption("--server-certificate-name")] string ServerCertificateName
) : AwsOptions
{
    [CliOption("--new-path")]
    public string? NewPath { get; set; }

    [CliOption("--new-server-certificate-name")]
    public string? NewServerCertificateName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}