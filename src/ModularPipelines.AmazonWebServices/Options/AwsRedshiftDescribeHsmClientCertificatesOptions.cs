using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-hsm-client-certificates")]
public record AwsRedshiftDescribeHsmClientCertificatesOptions : AwsOptions
{
    [CliOption("--hsm-client-certificate-identifier")]
    public string? HsmClientCertificateIdentifier { get; set; }

    [CliOption("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CliOption("--tag-values")]
    public string[]? TagValues { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}